using System;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db;
using Exico.HF.DbAccess.Db.Models;
using Hangfire;
using Hangfire.States;

namespace Exico.HF.DbAccess.Managers
{
    public class JobManager : IJobManager
    {
        private readonly ExicoHfDbContext _ctx;
        private readonly IBackgroundJobClient _hfBgClient;

        private JobManager()
        {
        }

        public JobManager(ExicoHfDbContext ctx, IBackgroundJobClient hfBgClient)
        {
            _ctx = ctx;
            _hfBgClient = hfBgClient;
        }

        public async Task<HfUserJob> Create(IFireAndForgetTaskOptions options, string name, string note)
        {           

            //create db record to get the db ID
            var userJob = await _CreateHfUserJob(options, name, note);
            //update options
            options.SetUserTaskId(userJob.Id);

            //create hangfire job and get hangfire job id
            var hfJobId = _hfBgClient.Enqueue<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null));

            var toBeUpdated = _ctx.HfUserJob.Where(x => x.Id == userJob.Id).FirstOrDefault();
            //update db
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.JsonOption = options.ToJson();
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _ctx.Update(toBeUpdated);
            await _ctx.SaveChangesAsync();
            //return job
            return toBeUpdated;
        }

        public async Task<HfUserJob> Create(IScheduledTaskOptions options, string name, string note)
        {
            //create db record to get the db ID
            var userJob = await _CreateHfUserJob(options, name, note);

            //update options
            options.SetUserTaskId(userJob.Id);

            //create hangfire job and get hangfire job id            
            var hfJobId = _hfBgClient.Schedule<IFireAndForgetTask>(x => x.Run(options.ToJson(),
                    JobCancellationToken.Null),
                    TimeZoneInfo.ConvertTimeToUtc(options.GetScheduledAt(),
                    TimeZoneInfo.FindSystemTimeZoneById(options.GetTimeZoneId())));

            var toBeUpdated = _ctx.HfUserJob.Where(x => x.Id == userJob.Id).FirstOrDefault();

            //update db
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.JsonOption = options.ToJson();
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _ctx.Update(toBeUpdated);
            await _ctx.SaveChangesAsync();
            //return job
            return toBeUpdated;
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
            return userJob;
        }
    }
}
