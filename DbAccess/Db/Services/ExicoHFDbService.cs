using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using Exico.HF.DbAccess.Extentions;
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

        public async Task<HfUserJob> Create(HfUserJob data)
        {
            data.CreatedOn = DateTimeOffset.Now;
            data.HfJobId = null;
            data.UpdatedOn = null;
            data.Id = 0;

            await _dbCtx.HfUserJob.AddAsync(data);
            await _dbCtx.SaveChangesAsync();

            return data;
        }

        public async Task<HfUserScheduledJob> Create(HfUserScheduledJob data)
        {
            await _dbCtx.HfUserScheduledJob.AddAsync(data);
            await _dbCtx.SaveChangesAsync();
            return data;
        }

        public async Task<HfUserRecurringJob> Create(HfUserRecurringJob data)
        {
            await _dbCtx.HfUserRecurringJob.AddAsync(data);
            await _dbCtx.SaveChangesAsync();
            return data;
        }

        public async Task<bool> Delete(int userJobId)
        {
            var data = _dbCtx.Find<HfUserJob>(userJobId);
            if (data == null) return false;
            else
            {
                if (data.IsScheduledJob())
                {
                    var rec = await _dbCtx.HfUserRecurringJob.FirstOrDefaultAsync(x => x.HfUserJobId == data.Id);
                    _dbCtx.HfUserRecurringJob.Remove(rec);
                }
                else if (data.IsFireAndForgetJob())
                {
                    var rec = await _dbCtx.HfUserScheduledJob.FirstOrDefaultAsync(x => x.HfUserJobId == data.Id);
                    _dbCtx.HfUserScheduledJob.Remove(rec);
                }

                _dbCtx.HfUserJob.Remove(data);

            }
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public async Task<HfUserJob> Get(int userJobId)
        {
            return await _dbCtx.HfUserJob.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userJobId);
        }

        public async Task<HfUserJob> SetHfJobId(int userJobId, string hfJobId)
        {
            var toBeUpdated = await _dbCtx.HfUserJob.FirstOrDefaultAsync(x => x.Id == userJobId);
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _dbCtx.Update(toBeUpdated);
            await _dbCtx.SaveChangesAsync();
            return toBeUpdated;
        }

        public async Task<HfUserJob> Update(HfUserJob data)
        {
            _dbCtx.HfUserJob.Attach(data);
            data.UpdatedOn = DateTimeOffset.UtcNow;
            _dbCtx.Update(data);
            await _dbCtx.SaveChangesAsync();
            return data;
        }

        public async Task<HfUserJob> UpdateStatus(int userJobId, JobStatus status)
        {
            var data = await Get(userJobId);
            data.Status = status;
            return await Update(data);
        }

        public void Dispose()
        {
            if (_dbCtx != null) _dbCtx.Dispose();
        }



    }
}