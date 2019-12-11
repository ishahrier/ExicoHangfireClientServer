using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using Exico.HF.DbAccess.Db.Services;
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
        private readonly IExicoHFDbService DbService;
        private readonly IBackgroundJobClient BgClient;
        private readonly IRecurringJobManager RecClient;
        private readonly ILogger<IJobManager> Logger;

        private JobManager() { }

        public JobManager(IExicoHFDbService dbSrv, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IJobManager> logger)
        {
            DbService = dbSrv;
            BgClient = hfBgClient;
            RecClient = hfRecClient;
            Logger = logger;
        }

        public async Task<HfUserJob> Create(IBaseTaskOptions options, string name, string note)
        {

            var userJob = await DbService.Create(name, note, options);
            options.SetUserTaskId(userJob.Id);
            var hfJobId = string.Empty;

            if (IsFireAndForgetJob(userJob))
            {
                var _options = options as IFireAndForgetTaskOptions;
                hfJobId = BgClient.Enqueue<IFireAndForgetTask>(x => x.Run(_options.ToJson(), JobCancellationToken.Null));
            }
            else if (IsScheduledJob(userJob))
            {
                var _options = options as IScheduledTaskOptions;
                hfJobId = BgClient.Schedule<IScheduledTask>(x => x.Run(_options.ToJson(),
                      JobCancellationToken.Null),
                      TimeZoneInfo.ConvertTimeToUtc(_options.GetScheduledAt(),
                      TimeZoneInfo.FindSystemTimeZoneById(_options.GetTimeZoneId())));

            }
            else if (IsRecurringJob(userJob))
            {
                var _options = options as IRecurringTaskOptions;
                hfJobId = Guid.NewGuid().ToString();
                RecClient.AddOrUpdate(hfJobId,
                    Job.FromExpression<IRecurringTask>((x) => x.Run(_options.ToJson(),
                    JobCancellationToken.Null)),
                    _options.GetCronExpression(),
                    TimeZoneInfo.FindSystemTimeZoneById(_options.GetTimeZoneId()));
            }
            else            
                throw new Exception("Invalid job type detected.");            

            return await DbService.Update(userJob.Id, hfJobId, options);
        }

        public async Task<bool> Cancel(long id)
        {
            var record = await DbService.Get(id);
            if (record != null)
            {
                if (IsFireAndForgetOrScheduled(record))
                    BgClient.Delete(record.HfJobId);
                else
                {
                    var job = JobStorage.Current
                        .GetConnection()
                        .GetRecurringJobs()
                        .FirstOrDefault(x => x.Id == record.HfJobId);

                    if (job != null)
                        BgClient.Delete(job.LastJobId);
                }
                await DbService.UpdateStatus(record, "Cancelled");
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(long id)
        {
            var record = await DbService.Get(id);
            if (record != null)
            {
                if (IsFireAndForgetOrScheduled(record))
                    BgClient.Delete(record.HfJobId);
                else if (IsRecurringJob(record))
                {
                    var job = JobStorage.Current
                                        .GetConnection()
                                        .GetRecurringJobs()
                                        .FirstOrDefault(x => x.Id == record.HfJobId);
                    if (job != null)
                        BgClient.Delete(job.LastJobId);
                    RecClient.RemoveIfExists(record.HfJobId);
                }
                else                
                    throw new Exception("Invalid job type detected.");
                
                return await DbService.Delete(record);
            }

            return false;
        }

        public async Task RunNow(long id)
        {
            var record = await DbService.Get(id);

            if (IsRecurringJob(record))
                RecClient.Trigger(record.HfJobId);
            else if (IsFireAndForgetOrScheduled(record))
                BgClient.ChangeState(record.HfJobId, new EnqueuedState());
            else
                throw new Exception("Invalid job type detected.");
        }

        #region Helper methods 
        private bool IsRecurringJob(HfUserJob record) => record?.JobType == JobType.Recurring;
        private bool IsScheduledJob(HfUserJob record) => record?.JobType == JobType.Scheduled;
        private bool IsFireAndForgetJob(HfUserJob record) => record?.JobType == JobType.FireAndForget;
        private bool IsFireAndForgetOrScheduled(HfUserJob record) => IsScheduledJob(record) || IsFireAndForgetJob(record);
        #endregion
    }
}