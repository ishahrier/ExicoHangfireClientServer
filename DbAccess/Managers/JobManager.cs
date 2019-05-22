using System;
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

        public async Task<HfUserJob>  Create(IFireAndForgetTaskOptions options, string name, string note)
        {
            if (!options.Validate()) throw new Exception("Options not valid.");

            //create db record to get the db ID
            var userJob = new HfUserJob()
            {
                CreatedOn = DateTimeOffset.UtcNow,
                Name = name,
                Note = note,
                UserId = options.UserId,
            };
            _ctx.HfUserJob.Add(userJob);
            var userJobId = await _ctx.SaveChangesAsync();

            //update options
            options.UserTaskId = userJobId;

            //create hangfire job and get hangfire job id
            var hfJobId = _hfBgClient.Enqueue<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null));

            //update db
            userJob.HfJobId = hfJobId;
            userJob.JsonOption = options.ToJson();
            userJob.UpdatedOn = DateTimeOffset.UtcNow;
            _ctx.Update(userJob);

            //return job
            return userJob;
        }

        public async Task<HfUserJob> Create(IScheduledTaskOptions options, string name, string note)
        {
            if (!options.Validate()) throw new Exception("Options not valid.");

            var userJob = new HfUserJob()
            {
                CreatedOn = DateTimeOffset.UtcNow,
                Name = name,
                Note = note,
                UserId = options.UserId,
            };
            _ctx.HfUserJob.Add(userJob);
            var userJobId = await _ctx.SaveChangesAsync();

            //update options
            options.UserTaskId = userJobId;

            //create hangfire job and get hangfire job id
            var hfJobId = _hfBgClient.Schedule<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null),TimeZoneInfo.ConvertTime(options.ScheduledAt,options.TimeZone ));

            //update db
            userJob.HfJobId = hfJobId;
            userJob.JsonOption = options.ToJson();
            userJob.UpdatedOn = DateTimeOffset.UtcNow;
            _ctx.Update(userJob);

            //return job
            return userJob;
        }
    }
}
