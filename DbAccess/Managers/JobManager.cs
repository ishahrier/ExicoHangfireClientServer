using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Services;
using Exico.HF.DbAccess.Extentions;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Managers
{
    /// <summary>
    /// Manages background jobs, it is a bridge between HFJob and UserJob
    /// </summary>
    public class JobManager : IManageJob
    {
        private readonly IExicoHfDbService _dbService;
        private readonly IBackgroundJobClient _bgClient;
        private readonly IRecurringJobManager _recClient;
        private readonly ILogger<IManageJob> _logger;

        private JobManager() { }

        public JobManager(IExicoHfDbService dbSrv, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IManageJob> logger)
        {
            _dbService = dbSrv;
            _bgClient = hfBgClient;
            _recClient = hfRecClient;
            _logger = logger;
        }

        public async Task<T> Create<T>(T t) where T : HfUserJobModel
        {
            T userJob = null;
            
            if (t is HfUserFireAndForgetJobModel)
            {
                userJob = await _dbService.Create<T>(t);
                _bgClient.Enqueue<IManageWork>(x => x.ExecuteWorkerAsync(userJob.ToWorkArguments(), JobCancellationToken.Null));
            }

            if (t is HfUserScheduledJobModel)
            {
                userJob = await _dbService.Create<T>(t);
                var casted = t.CastToScheduledJobModel();
                _bgClient.Schedule<IManageWork>(x => x.ExecuteWorkerAsync(userJob.ToWorkArguments(), JobCancellationToken.Null),
                      TimeZoneInfo.ConvertTimeToUtc(casted.ScheduledAt.DateTime.ToUnspecifiedDateTime(),
                      TimeZoneInfo.FindSystemTimeZoneById(userJob.TimeZoneId)));
            }

            if (t is HfUserRecurringJobModel)
            {
                _logger.LogInformation("Create recurring job {@data}", t);
                var initialRecJobId = Guid.NewGuid();
                userJob = await _dbService.Create<T>(t);
                var casted = t.CastToRecurringJobModel();
                _recClient.AddOrUpdate(initialRecJobId.ToString(),
                    Job.FromExpression<IManageWork>(x => x.ExecuteWorkerAsync(userJob.ToWorkArguments(), JobCancellationToken.Null)),
                     casted.CronExpression,
                    TimeZoneInfo.FindSystemTimeZoneById(userJob.TimeZoneId));
                await _dbService.SetRecurringJobInitialHfId(userJob.Id, initialRecJobId);
                userJob.HfJobId = initialRecJobId.ToString(); //Because we created the ID, so we know it.
            }

            if (userJob == null) throw new Exception("Could not create user job definition.");
            return userJob;
        }

        public async Task<bool> Cancel(int id)
        {
            var record = await _dbService.GetBaseData(id);
            if (record != null)
            {
                //await _dbService.UpdateStatus(record.Id, JobStatus.Cancelled, null);

                if (record.IsFireAndForgetOrScheduled())
                    _bgClient.Delete(record.HfJobId);
                else if (record.IsRecurringJob())
                {
                    var job = JobStorage.Current
                        .GetConnection()
                        .GetRecurringJobs()
                        .FirstOrDefault(x => x.Id == record.HfJobId);

                    if (job != null)
                        _bgClient.Delete(job.LastJobId);
                }
                else
                    throw new Exception("Cannot cancel, invalid job type detected.");

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var record = await _dbService.GetBaseData(id);
            if (record != null)
            {
                if (record.IsFireAndForgetOrScheduled())
                    _bgClient.Delete(record.HfJobId);
                else if (record.IsRecurringJob())
                {
                    var job = JobStorage.Current
                                        .GetConnection()
                                        .GetRecurringJobs()
                                        .FirstOrDefault(x => x.Id == record.HfJobId);
                    if (job != null)
                        _bgClient.Delete(job.LastJobId);
                    _recClient.RemoveIfExists(record.HfJobId);
                }
                else
                    throw new Exception("Cannot delete, invalid job type detected.");

                return await _dbService.Delete(record.Id);
            }

            return false;
        }

        public async Task RunNow(int id)
        {
            var record = await _dbService.GetBaseData(id);
            if (record.IsRecurringJob())
                _recClient.Trigger(record.HfJobId);
            else if (record.IsFireAndForgetOrScheduled())
            {
                if (record.HfJobId != null)
                    _bgClient.ChangeState(record.HfJobId, new EnqueuedState());
                else
                    throw new Exception("Cannot run the job, HFJob ID cannot be null");

            }
            else
                throw new Exception("Cannot run the job, invalid job type detected.");
        }

    }
}