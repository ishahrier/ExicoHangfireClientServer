//using Exico.HF.Common.Interfaces;
//using Exico.HF.DbAccess.Db.Models;
//using Exico.HF.DbAccess.Db.Services;
//using Hangfire;
//using Hangfire.Common;
//using Hangfire.States;
//using Hangfire.Storage;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Exico.HF.DbAccess.Managers
//{
//    public class JobManager : IJobManager
//    {
//        private readonly IExicoHFDbService _dbService;
//        private readonly IBackgroundJobClient _bgClient;
//        private readonly IRecurringJobManager _recClient;
//        private readonly ILogger<IJobManager> _logger;

//        private JobManager() { }

//        public JobManager(IExicoHFDbService dbSrv, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IJobManager> logger)
//        {
//            _dbService = dbSrv;
//            _bgClient = hfBgClient;
//            _recClient = hfRecClient;
//            _logger = logger;
//        }

//        public async Task<HfUserJob> Create(IBaseTaskOptions options, string name, string note)
//        {

//            var userJob = await _dbService.Create(name, note, options);
//            options.SetUserTaskId(userJob.Id);
//            var hfJobId = string.Empty;

//            if (IsFireAndForgetJob(userJob))
//            {
//                var _options = options as IFireAndForgetTaskOptions;
//                hfJobId = _bgClient.Enqueue<IFireAndForgetTask>(x => x.Run(_options.ToJson(), JobCancellationToken.Null));
//            }
//            else if (IsScheduledJob(userJob))
//            {
//                var _options = options as IScheduledTaskOptions;
//                hfJobId = _bgClient.Schedule<IScheduledTask>(x => x.Run(_options.ToJson(),
//                      JobCancellationToken.Null),
//                      TimeZoneInfo.ConvertTimeToUtc(_options.GetScheduledAt(),
//                      TimeZoneInfo.FindSystemTimeZoneById(_options.GetTimeZoneId())));

//            }
//            else if (IsRecurringJob(userJob))
//            {
//                var _options = options as IRecurringTaskOptions;
//                hfJobId = Guid.NewGuid().ToString();
//                _recClient.AddOrUpdate(hfJobId,
//                    Job.FromExpression<IRecurringTask>((x) => x.Run(_options.ToJson(),
//                    JobCancellationToken.Null)),
//                    _options.GetCronExpression(),
//                    TimeZoneInfo.FindSystemTimeZoneById(_options.GetTimeZoneId()));
//            }
//            else            
//                throw new Exception("Invalid job type detected.");            

//            return await _dbService.Update(userJob.Id, hfJobId, options);
//        }

//        public async Task<bool> Cancel(long id)
//        {
//            var record = await _dbService.Get(id);
//            if (record != null)
//            {
//                if (IsFireAndForgetOrScheduled(record))
//                    _bgClient.Delete(record.HfJobId);
//                else
//                {
//                    var job = JobStorage.Current
//                        .GetConnection()
//                        .GetRecurringJobs()
//                        .FirstOrDefault(x => x.Id == record.HfJobId);

//                    if (job != null)
//                        _bgClient.Delete(job.LastJobId);
//                }
//                await _dbService.UpdateStatus(record, "Cancelled");
//                return true;
//            }

//            return false;
//        }

//        public async Task<bool> Delete(long id)
//        {
//            var record = await _dbService.Get(id);
//            if (record != null)
//            {
//                if (IsFireAndForgetOrScheduled(record))
//                    _bgClient.Delete(record.HfJobId);
//                else if (IsRecurringJob(record))
//                {
//                    var job = JobStorage.Current
//                                        .GetConnection()
//                                        .GetRecurringJobs()
//                                        .FirstOrDefault(x => x.Id == record.HfJobId);
//                    if (job != null)
//                        _bgClient.Delete(job.LastJobId);
//                    _recClient.RemoveIfExists(record.HfJobId);
//                }
//                else                
//                    throw new Exception("Invalid job type detected.");
                
//                return await _dbService.Delete(record);
//            }

//            return false;
//        }

//        public async Task RunNow(long id)
//        {
//            var record = await _dbService.Get(id);

//            if (IsRecurringJob(record))
//                _recClient.Trigger(record.HfJobId);
//            else if (IsFireAndForgetOrScheduled(record))
//                _bgClient.ChangeState(record.HfJobId, new EnqueuedState());
//            else
//                throw new Exception("Invalid job type detected.");
//        }

//        #region Helper methods 
//        private bool IsRecurringJob(HfUserJob record) => record?.JobType == JobType.Recurring;
//        private bool IsScheduledJob(HfUserJob record) => record?.JobType == JobType.Scheduled;
//        private bool IsFireAndForgetJob(HfUserJob record) => record?.JobType == JobType.FireAndForget;
//        private bool IsFireAndForgetOrScheduled(HfUserJob record) => IsScheduledJob(record) || IsFireAndForgetJob(record);
//        #endregion
//    }
//}