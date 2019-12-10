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
        /// <summary>
        /// Create a user task (FnF, schduled or recurring)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="name"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        Task<HfUserJob> Create(IBaseTaskOptions options, string name, string note );

        /// <summary>
        /// Just stops the current execution. Doesn't delete the task.
        /// </summary>
        /// <param name="id">Task id of the user task , not HF job id</param>
        /// <returns></returns>
        Task<bool> Cancel(int id);

        /// <summary>
        /// Stops the current execution and deletes the job/task as well.
        /// </summary>
        /// <param name="id">Task id of the user task , not HF job id</param>
        /// <returns></returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Run a task manually
        /// </summary>
        /// <param name="id">Task id of the user task , not HF job id</param>
        /// <returns></returns>
        Task RunNow(int id);
    }
}
