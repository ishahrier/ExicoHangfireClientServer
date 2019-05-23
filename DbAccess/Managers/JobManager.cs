using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db;
using Exico.HF.DbAccess.Db.Models;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire.Common;
using Microsoft.EntityFrameworkCore;

namespace Exico.HF.DbAccess.Managers
{
    public class JobManager : IJobManager
    {
        private readonly ExicoHfDbContext _ctx;
        private readonly IBackgroundJobClient _hfBgClient;
        private readonly IRecurringJobManager _hfRecClient;
        private readonly ILogger<IJobManager> _logger;

        private JobManager()
        {
        }

        public JobManager(ExicoHfDbContext ctx, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IJobManager> logger)
        {
            _ctx = ctx;
            _hfBgClient = hfBgClient;
            _hfRecClient = hfRecClient;
            _logger = logger;
        }

        public async Task<HfUserJob> Create(IFireAndForgetTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await _CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);
            //create hangfire job and get hangfire job id
            var hfJobId = _hfBgClient.Enqueue<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null));

            return await _UpdateHfUserJob(options, userJob.Id, hfJobId);
        }

        public async Task<HfUserJob> Create(IScheduledTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await _CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);
            //create hangfire job and get hangfire job id            
            var hfJobId = _hfBgClient.Schedule<IScheduledTask>(x => x.Run(options.ToJson(),
                    JobCancellationToken.Null),
                    TimeZoneInfo.ConvertTimeToUtc(options.GetScheduledAt(),
                    TimeZoneInfo.FindSystemTimeZoneById(options.GetTimeZoneId())));

            return await _UpdateHfUserJob(options, userJob.Id, hfJobId);
        }

        public async Task<HfUserJob> Create(IRecurringTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await _CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);
            //create hangfire job and get hangfire job id            
            var hfJobId = Guid.NewGuid().ToString();
            _hfRecClient.AddOrUpdate(hfJobId,
                Job.FromExpression<IRecurringTask>((x) => x.Run(options.ToJson(),
                JobCancellationToken.Null)),
                options.GetCronExpression(),
                TimeZoneInfo.FindSystemTimeZoneById(options.GetTimeZoneId()));

            return await _UpdateHfUserJob(options, userJob.Id, hfJobId);
        }

        public async  Task<bool> DeleteJob(long userJobId)
        {
            var userJob = await _ctx.HfUserJob.Where(x => x.Id == userJobId).FirstOrDefaultAsync();
            if (userJob != null && userJob.Id == userJobId)
            {
                var hfId = userJob.HfJobId;
                switch (userJob.JobType)
                {
                    case JobType.FireAndForget:
                    case JobType.Scheduled:
                        _hfBgClient.Delete(hfId);
                        break;
                    case JobType.Recurring:
                        _hfRecClient.RemoveIfExists(hfId);
                        break;
                    default:
                        throw new Exception("Invalid job type detected");
                        
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private async Task<HfUserJob> _CreateHfUserJob(IBaseTaskOptions options, string name, string note)
        {
            if (!options.Validate()) throw new Exception("Options not valid.");

            //create db record to get the db ID
            var userJob = new HfUserJob()
            {
                CreatedOn = DateTimeOffset.UtcNow,
                Name = name,
                Note = note,
                UserId = options.GetUserId(),
                JobType = options.GetJobType()
            };

            await _ctx.HfUserJob.AddAsync(userJob);
            _ctx.SaveChangesAsync().Wait();

            //return newly created job
            return userJob;
        }


        private async Task<HfUserJob> _UpdateHfUserJob(IBaseTaskOptions options, long userJobId, string hfJobId)
        {
            var toBeUpdated = _ctx.HfUserJob.FirstOrDefault(x => x.Id == userJobId);
            //update db
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.JsonOption = options.ToJson();
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _ctx.Update(toBeUpdated);
            await _ctx.SaveChangesAsync();

            //return updated job
            return toBeUpdated;
        }
    }
}
