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
            var workArgs = new WorkArguments()
            {
                JobType = t.JobType,                
                WorkDataId = t.WorkDataId,
                WorkerClassName = t.WorkerClassName,
                WorkerAssemlyName = t.WorkerAssemblyName               
            };
            if (t is HfUserFireAndForgetJobModel)
            {
                userJob = await _dbService.Create<T>(t);
                workArgs.UserJobId = userJob.Id;
                _bgClient.Enqueue<IManageWork>(x => x.ExecWorker(workArgs, JobCancellationToken.Null));
                //job id should not be saved here, because it is run immidiately after creating
                // so there is a chance that the job will look for the id in the db before it is saved.
            }
            if (t is HfUserScheduledJobModel)
            {
                userJob = await _dbService.Create<T>(t);
                workArgs.UserJobId = userJob.Id;
                var casted = t.CastToScheduledJobModel();
                _bgClient.Schedule<IManageWork>(x => x.ExecWorker(workArgs, JobCancellationToken.Null),
                      TimeZoneInfo.ConvertTimeToUtc(casted.ScheduledAt.DateTime.ToUnspecifiedDateTime(),
                      TimeZoneInfo.FindSystemTimeZoneById(userJob.TimeZoneId)));
                //job id should not be saved here,  because if the delay is too short then the case might be similar to the above one.
            }
            if (t is HfUserRecurringJobModel)
            {
                var hfJobId = Guid.NewGuid().ToString();
                userJob = await _dbService.Create<T>(t);
                workArgs.UserJobId = userJob.Id;
                var casted = t.CastToRecurringJobModel();
                _recClient.AddOrUpdate(hfJobId,
                    Job.FromExpression<IManageWork>(x => x.ExecWorker(workArgs, JobCancellationToken.Null)),
                     casted.CronExpression,
                    TimeZoneInfo.FindSystemTimeZoneById(userJob.TimeZoneId));
                await _dbService.SetHfJobId(userJob.Id, hfJobId);
                userJob.HfJobId = hfJobId;
            }
            return userJob;
        }

        public async Task<bool> Cancel(int id)
        {
            var record = await _dbService.GetBase(id);
            if (record != null)
            {
                //await _dbService.UpdateStatus(record.Id, JobStatus.Cancelled, null);

                if (record.IsFireAndForgetOrScheduled())
                    _bgClient.Delete(record.HfJobId);
                else
                {
                    var job = JobStorage.Current
                        .GetConnection()
                        .GetRecurringJobs()
                        .FirstOrDefault(x => x.Id == record.HfJobId);

                    if (job != null)
                        _bgClient.Delete(job.LastJobId);
                }

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var record = await _dbService.GetBase(id);
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
                    throw new Exception("Invalid job type detected.");

                return await _dbService.Delete(record.Id);
            }

            return false;
        }

        public async Task RunNow(int id)
        {
            var record = await _dbService.GetBase(id);
            if (record.IsRecurringJob())
                _recClient.Trigger(record.HfJobId);
            else if (record.IsFireAndForgetOrScheduled())
            {
                if (record.HfJobId != null)
                    _bgClient.ChangeState(record.HfJobId, new EnqueuedState());
                else
                    throw new Exception("HFJob ID cannot be null");

            }
            else
                throw new Exception("Invalid job type detected.");
        }

    }
}