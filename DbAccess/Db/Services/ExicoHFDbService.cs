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
        private readonly ExicoHfDbContext _dbCtx;
        private readonly ILogger _logger;

        public ExicoHFDbService(ExicoHfDbContext ctx, ILogger<ExicoHfDbContext> logger)
        {
            _dbCtx = ctx;
            _logger = logger;
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
            await _dbCtx.HfUserJob.AddAsync(userJob);
            _dbCtx.SaveChangesAsync().Wait();

            return userJob;
        }

        public async Task<bool> Delete(HfUserJob job)
        {
            _dbCtx.HfUserJob.Remove(job);
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            if (_dbCtx != null) _dbCtx.Dispose();
        }

        public async Task<HfUserJob> Get(long userJobId) => await _dbCtx.HfUserJob.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userJobId);

        public async Task<HfUserJob> Update(long userJobId, string hfJobId, IBaseTaskOptions options)
        {
            var toBeUpdated = await _dbCtx.HfUserJob.FirstOrDefaultAsync(x => x.Id == userJobId);
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.JsonOption = options.ToJson();
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _dbCtx.Update(toBeUpdated);
            await _dbCtx.SaveChangesAsync();

            return toBeUpdated;
        }

        public async Task<HfUserJob> UpdateStatus(HfUserJob data, string status)
        {
            _dbCtx.HfUserJob.Attach(data);
            data.Status = status;
            data.UpdatedOn = DateTimeOffset.UtcNow;
            _dbCtx.Update(data);
            await _dbCtx.SaveChangesAsync();

            return data;
        }
    }
}