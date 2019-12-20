using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Models;
using Exico.HF.DbAccess.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public class ExicoHfDbService : IExicoHfDbService
    {
        private readonly IGenerateDbContext _ctxGenerator;
        private readonly ILogger _logger;

        public ExicoHfDbService(IGenerateDbContext ctxGenerator, ILogger<ExicoHfDbContext> logger)
        {
            _ctxGenerator = ctxGenerator;
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
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                await db.HfUserJob.AddAsync(dbModel);
                await db.SaveChangesAsync();
                return (HfUserFireAndForgetJobModel)dbModel.ToDomainModel();
            }
        }

        private async Task<HfUserScheduledJobModel> CreateScheduled(HfUserScheduledJobModel data)
        {
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var dbModel = data.ToDbModel();
                await db.HfUserScheduledJob.AddAsync(dbModel);
                await db.SaveChangesAsync();
                return dbModel.ToDomainModel();
            }
        }

        private async Task<HfUserRecurringJobModel> CreateRecurring(HfUserRecurringJobModel data)
        {

            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var dbModel = data.ToDbModel();
                await db.HfUserRecurringJob.AddAsync(dbModel);
                await db.SaveChangesAsync();
                return dbModel.ToDomainModel();
            }
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
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var record = await db.HfUserScheduledJob
                .AsNoTracking()
                .Include(x => x.HfUserJob)
                .FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                if (record != null) return record.ToDomainModel();
                else return null;
            }
        }

        private async Task<HfUserRecurringJobModel> GetRecurring(int userJobId)
        {
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var record = await db.HfUserRecurringJob
                .AsNoTracking()
                .Include(x => x.HfUserJob)
                .FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                if (record != null) return record.ToDomainModel();
                else return null;
            }
        }
        public async Task<HfUserJobModel> GetBase(int userJobId)
        {
            var data = (await Get(userJobId));
            if (data.IsFireAndForgetJob())
            {
                return data.ToDomainModel();
            }
            else if (data.IsScheduledJob())
            {
                var _data = await Get<HfUserScheduledJobModel>(userJobId);
                return _data;
            }
            else if (data.IsRecurringJob())
            {
                var _data = await Get<HfUserRecurringJobModel>(userJobId);
                return _data;
            }

            throw new Exception("Invalid job type detected");
        }
        private async Task<HfUserJob> Get(int userJobId, bool tracking = false)
        {
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var query = tracking ? db.HfUserJob : db.HfUserJob.AsNoTracking();
                return await query.FirstOrDefaultAsync(x => x.Id == userJobId);
            }
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
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var toBeUpdated = await Get(userJobId, true);
                toBeUpdated.HfJobId = hfJobId;
                toBeUpdated.UpdatedOn = DateTimeOffset.UtcNow;
                db.Update(toBeUpdated);
                return await db.SaveChangesAsync() > 0;
            }
        }
        public async Task<bool> SetRecurringLastRunJobId(int userJobId, string lastRunJobId)
        {
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                var toBeUpdated = await db.HfUserRecurringJob.Include(x=>x.HfUserJob).FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                toBeUpdated.LastHfJobId = lastRunJobId;                
                db.Update(toBeUpdated);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateRecurringNextRun(int userJobId, DateTime nextRun)
        {
            using(var db = _ctxGenerator.GenerateNewContext())
            {
                var toBeUpdated = await db.HfUserRecurringJob.Include(x => x.HfUserJob).FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                toBeUpdated.NextRun = nextRun;
                db.Update(toBeUpdated);
                return await db.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> Delete(int userJobId)
        {
            var job = await Get(userJobId, true);

            using (var db = _ctxGenerator.GenerateNewContext())
            {
                if (job != null)
                {
                    if (job.IsFireAndForgetJob())
                    {
                        db.HfUserJob.Remove(job);
                    }
                    if (job.IsScheduledJob())
                    {
                        var scheduled = await db.HfUserScheduledJob.FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                        db.HfUserScheduledJob.Remove(scheduled);
                    }
                    if (job.IsRecurringJob())
                    {
                        var rec = await db.HfUserRecurringJob.FirstOrDefaultAsync(x => x.HfUserJobId == userJobId);
                        db.HfUserRecurringJob.Remove(rec);
                    }

                    return await db.SaveChangesAsync() > 0;
                }

                return false;
            }
        }
 
        public async Task<string> GetHfJobId(int userJobId)
        {
            var job = await Get(userJobId, false);
            if (job != null) return job.HfJobId;
            return null;
        }

        public async Task<bool> UpdateStatus(int userJobId, JobStatus status, string hfJobId)
        {
            var data = await Get(userJobId, true);
            using (var db = _ctxGenerator.GenerateNewContext())
            {
                if (data.IsFireAndForgetOrScheduled())
                {
                    db.HfUserJob.Attach(data);
                    data.Status = status;
                    return await db.SaveChangesAsync() > 0;
                }
                 if (data.IsRecurringJob())
                {
                    var recData = db.HfUserRecurringJob.Where(x => x.HfUserJobId == userJobId).FirstOrDefault();
                    recData.LastHfJobId = hfJobId;
                    return await db.SaveChangesAsync() > 0;

                }
                return false;
            }
        } 

        #endregion

    }
}