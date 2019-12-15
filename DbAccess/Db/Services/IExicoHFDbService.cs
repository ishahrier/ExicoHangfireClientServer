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
        Task<bool> SetHfJobId(int userJobId, string hfJobId);


        Task<HfUserFireAndForgetJobModel> GetFnf(int userJobId);
        Task<HfUserScheduledJobModel> GetScheduled(int userJobId);
        Task<HfUserRecurringJobModel> GetRecurring(int userJobId);


        Task<HfUserFireAndForgetJobModel> CreateFnf(HfUserFireAndForgetJobModel data);
        Task<HfUserScheduledJobModel> CreateScheduled(HfUserScheduledJobModel data);
        Task<HfUserRecurringJobModel> CreateRecurring(HfUserRecurringJobModel data);

        Task<HfUserFireAndForgetJobModel> UpdateFnf(HfUserFireAndForgetJobModel data);
        Task<HfUserScheduledJobModel> UpdateScheduled(HfUserScheduledJobModel data);
        Task<HfUserRecurringJobModel> UpdateRecurring(HfUserRecurringJobModel data);

        Task<bool> UpdateStatus(int userJobId, JobStatus status);
        Task<bool> DeleteFnf(int userJobId);
        Task<bool> DeleteScheduled(int userJobId);
        Task<bool> DeleteRecurring(int userJobId);
    }
}