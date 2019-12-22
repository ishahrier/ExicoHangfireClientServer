using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using System;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public interface IExicoHfDbService
    {
        /// <summary>
        /// This method sets the HF recurring job definition guid. The initial id of a recurring job.
        /// When recurring user job is created, an Id is created for parent HF job definition.
        /// It is unlike the bgjobId , bgJobId is created for each time the recurring task is enqueued
        /// as a back ground job. And it is updated along with statis using <seealso cref="UpdateStatusBgJobId(int, JobStatus, string)"/>
        /// from within the <seealso cref="Exico.HF.DbAccess.Filter.DefaultExicoHfFilter"/>
        /// </summary>
        /// <param name="userJobId"></param>
        /// <param name="id"></param>
        /// <returns>tru if succedded, false otherwise</returns>
        Task<bool> SetRecurringJobInitialHfId(int userJobId, Guid id);
        Task<T> Get<T>(int userJobId) where T : HfUserJobModel;
        /// <summary>
        /// Returns common base data for any kind of job. Use this when you need to know only the base information
        /// and you do not know what type of job it is and also do not care about the job type.
        /// </summary>
        /// <param name="userJobId"></param>
        /// <returns>Returns <seealso cref="HfUserJobModel"/></returns>
        Task<HfUserJobModel> GetBaseData(int userJobId);
        Task<T> Create<T>(T data) where T : HfUserJobModel;
        Task<T> Update<T>(T t) where T : HfUserJobModel;

        /// <summary>
        /// This is mainly called from the <see cref="Filter.DefaultExicoHfFilter"/>
        /// to update the status on the job is enqueued with has a status.
        /// </summary>
        /// <param name="userJobId"></param>
        /// <param name="status"></param>
        /// <param name="hfJobId"></param>
        /// <returns>tru if succedded, false otherwise</returns>
        Task<bool> UpdateStatusBgJobId(int userJobId, JobStatus status, string hfJobId);

        Task<bool> Delete(int userJobId);
        /// <summary>
        /// Rweturns the initial parent GUID of the HF Recurring job.
        /// </summary>
        /// <param name="userJobId"></param>
        /// <returns>The guid</returns>
        Task<Guid> GetRecurringInitialHfJobId(int userJobId);
        Task<bool> SetRecurringLastRunJobId(int userJobId, string lastRunJobId);
        Task<bool> UpdateRecurringNextRun(int userJobId,  DateTime nextRun);


    }
}