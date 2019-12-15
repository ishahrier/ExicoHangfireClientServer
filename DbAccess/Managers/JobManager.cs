//using Exico.HF.Common.DomainModels;
//using Exico.HF.Common.Enums;
//using Exico.HF.Common.Interfaces;
//using Exico.HF.DbAccess.Db.Models;
//using Exico.HF.DbAccess.Db.Services;
//using Exico.HF.DbAccess.Extentions;
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
//    public class JobManager : IManageJob
//    {
//        private readonly IExicoHfDbService _dbService;
//        private readonly IBackgroundJobClient _bgClient;
//        private readonly IRecurringJobManager _recClient;
//        private readonly ILogger<IManageJob> _logger;

//        private JobManager() { }

//        public JobManager(IExicoHFDbService dbSrv, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IManageJob> logger)
//        {
//            _dbService = dbSrv;
//            _bgClient = hfBgClient;
//            _recClient = hfRecClient;
//            _logger = logger;
//        }

//        public async Task<HfUserJobModel> Create(HfUserJobModel data)
//        {
//            var userJob = await _dbService.Create(data.ToDbModel());

//            if (userJob != null)
//            {
//                var hfJobId = string.Empty;

//                if (userJob.IsFireAndForgetJob())
//                {
//                    hfJobId = _bgClient.Enqueue<IWork>(x => x.DoWork(userJob.Id, userJob.WorkDataId, JobCancellationToken.Null));
//                }
//                else if (userJob.IsScheduledJob())
//                {
//                    var userScheduledJob = new HfUserScheduledJob();
//                    hfJobId = _bgClient.Schedule<IWork>(x => x.DoWork(userJob.Id, userJob.WorkDataId, JobCancellationToken.Null),
//                          TimeZoneInfo.ConvertTimeToUtc(userScheduledJob.ScheduledAt.DateTime.ToUnspecifiedDateTime(),
//                          TimeZoneInfo.FindSystemTimeZoneById(userJob.TimeZoneId)));
//                }
//                else if (userJob.IsRecurringJob())
//                {
//                    hfJobId = Guid.NewGuid().ToString();
//                    var userRecurringJob = new HfUserRecurringJob();
//                    _recClient.AddOrUpdate(hfJobId,
//                        Job.FromExpression<IWork>(x => x.DoWork(userJob.Id, userJob.WorkDataId, JobCancellationToken.Null)),
//                        userRecurringJob.CronExpression,
//                        TimeZoneInfo.FindSystemTimeZoneById(userJob.TimeZoneId));
//                }
//                else
//                    throw new Exception("Invalid job type detected.");

//                return (await _dbService.SetHfJobId(userJob.Id, hfJobId)).ToDomainModel();
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public async Task<bool> Cancel(int id)
//        {
//            var record = await _dbService.Get(id);
//            if (record != null)
//            {
//                if (record.IsFireAndForgetOrScheduled())
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
//                await _dbService.UpdateStatus(record.Id, JobStatus.Cancelled);
//                return true;
//            }

//            return false;
//        }

//        public async Task<bool> Delete(int id)
//        {
//            var record = await _dbService.Get(id);
//            if (record != null)
//            {
//                if (record.IsFireAndForgetOrScheduled())
//                    _bgClient.Delete(record.HfJobId);
//                else if (record.IsRecurringJob())
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

//                return await _dbService.Delete(record.Id);
//            }

//            return false;
//        }

//        public async Task RunNow(int id)
//        {
//            var record = await _dbService.Get(id);
//            if (record.IsRecurringJob())
//                _recClient.Trigger(record.HfJobId);
//            else if (record.IsFireAndForgetOrScheduled())
//                _bgClient.ChangeState(record.HfJobId, new EnqueuedState());
//            else
//                throw new Exception("Invalid job type detected.");
//        }

//    }
//}