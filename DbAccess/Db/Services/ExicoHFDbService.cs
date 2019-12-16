using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
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

        #region Create
        public async Task<T> Create<T>(T data) where T : HfUserJobModel
        {
            T ret = null;
            data.CreatedOn = DateTimeOffset.Now;
            if (data is HfUserFireAndForgetJobModel)
            {
                var _ret = await CreateFnf(data.CastToFireAndForgetJobModel());
                ret = _ret.CastToJobModel<T>();
            }
            if (data is HfUserScheduledJobModel)
            {
                var _ret = await CreateScheduled(data.CastToScheduledJobModel());
                ret = _ret.CastToJobModel<T>();
            }
            if (data is HfUserRecurringJobModel)
            {
                var _ret = await CreateRecurring(data.CastToRecurringJobModel());
                ret = _ret.CastToJobModel<T>();
            }

            return ret;
        }
        private async Task<HfUserFireAndForgetJobModel> CreateFnf(HfUserFireAndForgetJobModel data)
        {
            var dbModel = data.ToDbModel();
            await _dbCtx.HfUserJob.AddAsync(dbModel);
            await _dbCtx.SaveChangesAsync();
            return (HfUserFireAndForgetJobModel)dbModel.ToDomainModel();
        }

        private async Task<HfUserScheduledJobModel> CreateScheduled(HfUserScheduledJobModel data)
        {
            var dbModel = data.ToDbModel();
            await _dbCtx.HfUserScheduledJob.AddAsync(dbModel);
            await _dbCtx.SaveChangesAsync();
            return dbModel.ToDomainModel();
        }

        private async Task<HfUserRecurringJobModel> CreateRecurring(HfUserRecurringJobModel data)
        {
            var dbModel = data.ToDbModel();
            await _dbCtx.HfUserRecurringJob.AddAsync(dbModel);
            await _dbCtx.SaveChangesAsync();
            return dbModel.ToDomainModel();
        }

        #endregion

        #region Get
        public async Task<T> Get<T>(int userJobId) where T : HfUserJobModel
        {
            var t = typeof(T);
            T ret = null;
            if (t == typeof(HfUserFireAndForgetJobModel))
            {
                var _ret = await GetFnf(userJobId);
                ret = _ret.CastToJobModel<T>();
            }

            if (t == typeof(HfUserScheduledJobModel))
            {
                var _ret = await GetScheduled(userJobId);
                ret = _ret.CastToJobModel<T>();
            }

            if (t == typeof(HfUserRecurringJobModel))
            {
                var _ret = await GetRecurring(userJobId);
                ret = _ret.CastToJobModel<T>();
            }

            
            return ret;
        }
        private async Task<HfUserFireAndForgetJobModel> GetFnf(int userJobId)
        {
            var record = await Get(userJobId);
            if (record != null) return (HfUserFireAndForgetJobModel)record.ToDomainModel();
            else return null;
        }

        private async Task<HfUserScheduledJobModel> GetScheduled(int userJobId)
        {
            var record = await _dbCtx.HfUserScheduledJob
                .AsNoTracking()
                .Include(x => x.HfUserJob)
                .FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
            if (record != null) return record.ToDomainModel();
            else return null;
        }

        private async Task<HfUserRecurringJobModel> GetRecurring(int userJobId)
        {
            var record = await _dbCtx.HfUserRecurringJob
                .AsNoTracking()
                .Include(x => x.HfUserJob)
                .FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
            if (record != null) return record.ToDomainModel();
            else return null;
        }
        public async Task<HfUserJobModel> GetBase(int userJobId) => (await Get(userJobId)).ToDomainModel();
        private async Task<HfUserJob> Get(int userJobId,bool tracking=false)
        {
            var query = tracking ?  _dbCtx.HfUserJob : _dbCtx.HfUserJob.AsNoTracking();     
            return await query.FirstOrDefaultAsync(x => x.Id == userJobId);
        }

        #endregion

        #region Update
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

        public Task<T> Update<T>(T t) where T : HfUserJobModel
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Others
        public async Task<bool> SetHfJobId(int userJobId, string hfJobId)
        {
            var toBeUpdated = await Get(userJobId, true);
            toBeUpdated.HfJobId = hfJobId;
            toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
            _dbCtx.Update(toBeUpdated);
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStatus(int userJobId, JobStatus status)
        {
            var data = await Get(userJobId, true);
            data.Status = status;
            return await _dbCtx.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int userJobId)
        {
            var job = await Get(userJobId, true);

            if (job != null)
            {
                if (job.IsFireAndForgetJob())
                {
                    _dbCtx.HfUserJob.Remove(job);
                }
                if (job.IsScheduledJob())
                {
                    var scheduled = await _dbCtx.HfUserScheduledJob.FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                    _dbCtx.HfUserScheduledJob.Remove(scheduled);
                }
                if (job.IsRecurringJob())
                {
                    var rec = await _dbCtx.HfUserRecurringJob.FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                    _dbCtx.HfUserRecurringJob.Remove(rec);
                }

                return await _dbCtx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public void Dispose()
        {
            if (_dbCtx != null) _dbCtx.Dispose();
        }

        public async Task<string> GetHfJobId(int userJobId)
        {
            var job = await Get(userJobId, false);
            if (job != null) return job.HfJobId;
            return null;
        }


        #endregion

    }
}