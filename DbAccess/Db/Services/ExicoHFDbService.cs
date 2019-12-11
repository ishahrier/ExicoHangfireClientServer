using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public class ExicoHFDbService : IExicoHFDbService
    {
        private readonly ExicoHfDbContext dbCtx;
        private readonly ILogger logger;

        public ExicoHFDbService(ExicoHfDbContext ctx, ILogger<ExicoHfDbContext> logger)
        {
            dbCtx = ctx;
            this.logger = logger;
        }

        public async Task<HfUserJob> Create(string name, string note, IBaseTaskOptions options)
        {
            if (!options.Validate()) throw new Exception("Options not valid.");
            var userJob = new HfUserJob()
            {
                CreatedOn = DateTimeOffset.UtcNow,
                Name = name,
                Note = note,
                UserId = options.GetUserId(),
                JobType = options.GetJobType()
            };
            await dbCtx.HfUserJob.AddAsync(userJob);
            dbCtx.SaveChangesAsync().Wait();

            return userJob;
        }

        public async Task<bool> Delete(HfUserJob job)
        {
            dbCtx.HfUserJob.Remove(job);
            return await dbCtx.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            if (dbCtx != null) dbCtx.Dispose();
        }

        public async Task<HfUserJob> Get(long userJobId) => await dbCtx.HfUserJob.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userJobId);

        public async Task<HfUserJob> Update(long userJobId, string hfJobId, IBaseTaskOptions options)
        {
            var toBeUpdated = await dbCtx.HfUserJob.FirstOrDefaultAsync(x => x.Id == userJobId);
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.JsonOption = options.ToJson();
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            dbCtx.Update(toBeUpdated);
            await dbCtx.SaveChangesAsync();

            return toBeUpdated;
        }

        public async Task<HfUserJob> UpdateStatus(HfUserJob data, string status)
        {
            dbCtx.HfUserJob.Attach(data);
            data.Status = status;
            data.UpdatedOn = DateTimeOffset.UtcNow;
            dbCtx.Update(data);
            await dbCtx.SaveChangesAsync();

            return data;
        }
    }
}