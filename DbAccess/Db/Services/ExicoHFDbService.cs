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
    public class ExicoHfDbService : IExicoHfDbService
    {
        private readonly ExicoHfDbContext _dbCtx;
        private readonly ILogger _logger;

        public ExicoHfDbService(ExicoHfDbContext ctx, ILogger<ExicoHfDbContext> logger)
        {
            _dbCtx = ctx;
            _logger = logger;
        }

        public async Task<HfUserFireAndForgetJobModel> CreateFnf(HfUserFireAndForgetJobModel data)
        {
            var dbModel = data.ToDbModel();
            await _dbCtx.HfUserJob.AddAsync(dbModel);
            return (HfUserFireAndForgetJobModel)dbModel.ToDomainModel();
        }

        public async Task<HfUserScheduledJobModel> CreateScheduled(HfUserScheduledJobModel data)
        {
            var dbModel = data.ToDbModel();
            await _dbCtx.HfUserScheduledJob.AddAsync(dbModel);
            return  dbModel.ToDomainModel();
        }

        public async Task<HfUserRecurringJobModel> CreateRecurring(HfUserRecurringJobModel data)
        {
            var dbModel = data.ToDbModel();
            await _dbCtx.HfUserRecurringJob.AddAsync(dbModel);
            return  dbModel.ToDomainModel();
        }

        public async Task<bool> SetHfJobId(int userJobId, string hfJobId)
        {
            var toBeUpdated = await _dbCtx.HfUserJob.FirstOrDefaultAsync(x => x.Id == userJobId);
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _dbCtx.Update(toBeUpdated);
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public Task<HfUserFireAndForgetJobModel> UpdateFnf(HfUserFireAndForgetJobModel data)
        {
            throw new NotImplementedException();
        }

        public Task<HfUserScheduledJobModel> UpdateScheduled(HfUserScheduledJobModel data)
        {
            throw new NotImplementedException();
        }

        public Task<HfUserRecurringJobModel> UpdateRecurring(HfUserRecurringJobModel data)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatus(int userJobId, JobStatus status)
        {
            var data = await _dbCtx.HfUserJob.FindAsync(userJobId);
            data.Status = status;
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public async Task<HfUserFireAndForgetJobModel> GetFnf(int userJobId)
        {
            var record = await _dbCtx.HfUserJob
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userJobId);
            if (record != null) return (HfUserFireAndForgetJobModel)record.ToDomainModel();
            else return null;
        }

        public async Task<HfUserScheduledJobModel> GetScheduled(int userJobId)
        {
            var record = await _dbCtx.HfUserScheduledJob
                .AsNoTracking()
                .Include(x => x.HfUserJob)
                .FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
            if (record != null) return record.ToDomainModel();
            else return null;
        }

        public async Task<HfUserRecurringJobModel> GetRecurring(int userJobId)
        {
            var record = await _dbCtx.HfUserRecurringJob
                .AsNoTracking()
                .Include(x => x.HfUserJob)
                .FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
            if (record != null) return record.ToDomainModel();
            else return null;
        }

        public async Task<bool> DeleteFnf(int userJobId)
        {
            var data = await _dbCtx.HfUserJob.FirstOrDefaultAsync(x => x.Id == userJobId);
            if (data == null) return false;
            _dbCtx.HfUserJob.Remove(data);
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteScheduled(int userJobId)
        {
            var data = await _dbCtx.HfUserScheduledJob.FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
            if (data == null) return false;
            _dbCtx.HfUserScheduledJob.Remove(data);
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRecurring(int userJobId)
        {
            var data = await _dbCtx.HfUserRecurringJob.FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
            if (data == null) return false;
            _dbCtx.HfUserRecurringJob.Remove(data);
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            if (_dbCtx != null) _dbCtx.Dispose();
        }
    }
}