using Hangfire;

namespace Exico.HF.Common.Interfaces
{
    /// <summary>
    /// A 'work' is a definied steps that is needed to be executed.
    /// A work has an associated table, that contains work paramaters.
    /// 
    /// A work can be, i.e. 'delete all products that has zero unit on hand' or 'Email a report of revenue'
    /// 
    /// A 'work' is not a UserJob or HangFire job. A UserJob is created to 
    /// schedule a 'Work'
    /// </summary>
    public interface IWork
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId">User job ID, created by user</param>
        /// <param name="dataId">Id of the actual work data </param>
        /// <param name="cancellationToken">hangfire cancellation token</param>
        public void DoWork(uint jobId, uint dataId, JobCancellationToken cancellationToken);

        /// <summary>
        /// Data table needed by the work to understand WHAT to do , it is can been as INPUTS of a work.
        /// </summary>
        /// <returns></returns>
        public string GetDataTableName();
    }
}
