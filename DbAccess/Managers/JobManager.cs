using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
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
    public class JobManager : IJobManager
    {
        private readonly IExicoHFDbService _dbService;
        private readonly IBackgroundJobClient _bgClient;
        private readonly IRecurringJobManager _recClient;
        private readonly ILogger<IJobManager> _logger;

        private JobManager() { }

        public JobManager(IExicoHFDbService dbSrv, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IJobManager> logger)
        {
            _dbService = dbSrv;
            _bgClient = hfBgClient;
            _recClient = hfRecClient;
            _logger = logger;
        }

        public async Task<HfUserJobModel> Create(HfUserJobModel data)
        {
            var userJob = await _dbService.Create(data.ToDbModel());

            if (userJob != null)
            {
                var hfJobId = string.Empty;
                if (userJob.IsFireAndForgetJob())
                {
                    var _options = options as IFireAndForgetTaskOptions;
                    hfJobId = _bgClient.Enqueue<IFireAndForgetTask>(x => x.Run(_options.ToJson(), JobCancellationToken.Null));
                }
                else if (userJob.IsScheduledJob())
                {
                    var _options = options as IScheduledTaskOptions;
                    hfJobId = _bgClient.Schedule<IScheduledTask>(x => x.Run(_options.ToJson(),
                          JobCancellationToken.Null),
                          TimeZoneInfo.ConvertTimeToUtc(_options.GetScheduledAt(),
                          TimeZoneInfo.FindSystemTimeZoneById(_options.GetTimeZoneId())));

                }
                else if (userJob.IsRecurringJob())
                {
                    var _options = options as IRecurringTaskOptions;
                    hfJobId = Guid.NewGuid().ToString();
                    _recClient.AddOrUpdate(hfJobId,
                        Job.FromExpression<IRecurringTask>((x) => x.Run(_options.ToJson(),
                        JobCancellationToken.Null)),
                        _options.GetCronExpression(),
                        TimeZoneInfo.FindSystemTimeZoneById(_options.GetTimeZoneId()));
                }
                else
                    throw new Exception("Invalid job type detected.");

                return (await _dbService.SetHfJobId(userJob.Id, hfJobId)).ToDomainModel();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Cancel(int id)
        {
            var record = await _dbService.Get(id);
            if (record != null)
            {
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
                await _dbService.UpdateStatus(record.Id, JobStatus.Cancelled);
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var record = await _dbService.Get(id);
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
            var record = await _dbService.Get(id);
            if (record.IsRecurringJob())
                _recClient.Trigger(record.HfJobId);
            else if (record.IsFireAndForgetOrScheduled())
                _bgClient.ChangeState(record.HfJobId, new EnqueuedState());
            else
                throw new Exception("Invalid job type detected.");
        }

    }
}