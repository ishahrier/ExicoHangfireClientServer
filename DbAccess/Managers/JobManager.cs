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
using Hangfire.Storage;

namespace Exico.HF.DbAccess.Managers
{
    public class JobManager : IJobManager
    {
        private readonly ExicoHfDbContext DbCtx;
        private readonly IBackgroundJobClient BgClient;
        private readonly IRecurringJobManager RecClient;
        private readonly ILogger<IJobManager> Logger;

        private JobManager()
        {
        }

        public JobManager(ExicoHfDbContext ctx, IBackgroundJobClient hfBgClient, IRecurringJobManager hfRecClient, ILogger<IJobManager> logger)
        {
            DbCtx = ctx;
            BgClient = hfBgClient;
            RecClient = hfRecClient;
            Logger = logger;
        }

        public async Task<HfUserJob> Create(IFireAndForgetTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);
            //create hangfire job and get hangfire job id
            var hfJobId = BgClient.Enqueue<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null));

            return await UpdateHfUserJob(options, userJob.Id, hfJobId);
        }

        public async Task<HfUserJob> Create(IScheduledTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);
            //create hangfire job and get hangfire job id            
            var hfJobId = BgClient.Schedule<IScheduledTask>(x => x.Run(options.ToJson(),
                    JobCancellationToken.Null),
                    TimeZoneInfo.ConvertTimeToUtc(options.GetScheduledAt(),
                    TimeZoneInfo.FindSystemTimeZoneById(options.GetTimeZoneId())));

            return await UpdateHfUserJob(options, userJob.Id, hfJobId);
        }

        public async Task<HfUserJob> Create(IRecurringTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);
            //create hangfire job and get hangfire job id            
            var hfJobId = Guid.NewGuid().ToString();
            RecClient.AddOrUpdate(hfJobId,
                Job.FromExpression<IRecurringTask>((x) => x.Run(options.ToJson(),
                JobCancellationToken.Null)),
                options.GetCronExpression(),
                TimeZoneInfo.FindSystemTimeZoneById(options.GetTimeZoneId()));

            return await UpdateHfUserJob(options, userJob.Id, hfJobId);
        }

        public async Task<bool> DeleteJob(long userJobId)
        {
            var userJob = await DbCtx.HfUserJob.Where(x => x.Id == userJobId).FirstOrDefaultAsync();
            if (userJob != null && userJob.Id == userJobId)
            {
                var hfId = userJob.HfJobId;
                switch (userJob.JobType)
                {
                    case JobType.FireAndForget:
                    case JobType.Scheduled:
                        BgClient.Delete(hfId);
                        break;
                    case JobType.Recurring:
                        RecClient.RemoveIfExists(hfId);
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

        private async Task<HfUserJob> CreateHfUserJob(IBaseTaskOptions options, string name, string note)
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

            await DbCtx.HfUserJob.AddAsync(userJob);
            DbCtx.SaveChangesAsync().Wait();

            //return newly created job
            return userJob;
        }

        private async Task<HfUserJob> GetHfUserJob(int id) => await DbCtx.HfUserJob.FirstOrDefaultAsync(x => x.Id == id);

        private bool IsRecurringJob(HfUserJob record) => record?.Status == JobType.Recurring;
        private bool IsScheduledJob(HfUserJob record) => record?.Status == JobType.Scheduled;
        private bool IsFireAndForgetJob(HfUserJob record) => record?.Status == JobType.FireAndForget;
        private bool IsFireAndForgetOrScheduled(HfUserJob record) => IsScheduledJob(record) || IsFireAndForgetJob(record);


        private async Task<HfUserJob> UpdateHfUserJob(IBaseTaskOptions options, long userJobId, string hfJobId)
        {
            var toBeUpdated = DbCtx.HfUserJob.FirstOrDefault(x => x.Id == userJobId);
            //update db
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.JsonOption = options.ToJson();
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            DbCtx.Update(toBeUpdated);
            await DbCtx.SaveChangesAsync();

            //return updated job
            return toBeUpdated;
        }

        public async Task<bool> Cancel(int id)
        {
            var record = await GetHfUserJob(id);
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

                record.Status = "Cancelled"; //from enum
                await DbCtx.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var record = await GetHfUserJob(id);
            if (record != null)
            {
                if (IsFireAndForgetOrScheduled(record))
                    BgClient.Delete(record.HfJobId);
                else
                    RecClient.RemoveIfExists(record.HfJobId);
                DbCtx.HfUserJob.Remove(record);
                await DbCtx.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
