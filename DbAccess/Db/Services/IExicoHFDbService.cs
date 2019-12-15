using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public interface IExicoHfDbService : IDisposable
    {
        Task<HfUserJobModel> Get(int userJobId);
        Task<bool> SetHfJobId(int userJobId, string hfJobId);

        Task<HfUserFireAndForgetJobModel> Create(HfUserFireAndForgetJobModel data);
        Task<HfUserScheduledJobModel> Create(HfUserScheduledJobModel data);
        Task<HfUserRecurringJobModel> Create(HfUserRecurringJobModel data);

        Task<HfUserJobModel> UpdateStatus(int userJobId, JobStatus status);
        Task<HfUserFireAndForgetJobModel> Update(HfUserFireAndForgetJobModel data);
        Task<HfUserScheduledJobModel> Update(HfUserScheduledJobModel data);
        Task<HfUserRecurringJobModel> Update(HfUserRecurringJobModel data);
            
        Task<bool> Delete(int userJobId);
    }
}
