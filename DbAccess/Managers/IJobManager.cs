using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using Hangfire;


namespace Exico.HF.DbAccess.Managers
{
    public interface IJobManager
    {
        Task<HfUserJob> Create(IFireAndForgetTaskOptions options, string name, string note);
        Task<HfUserJob> Create(IScheduledTaskOptions options, string name, string note);
        Task<HfUserJob> Create(IRecurringTaskOptions options, string name, string note);

        /// <summary>
        /// Just stops the current execution. Doesn't delete the task.
        /// </summary>
        /// <param name="id">Task id of the user task , not HF job id</param>
        /// <returns></returns>
        Task<bool> Cancel(int id);

        /// <summary>
        /// Stops the current execution and deletes the job as well.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(int id);
    }
}
