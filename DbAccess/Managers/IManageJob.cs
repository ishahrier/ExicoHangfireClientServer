using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using System.Threading.Tasks;


namespace Exico.HF.DbAccess.Managers
{
    public interface IManageJob
    {
        /// <summary>
        /// Create a user task (FnF, schduled or recurring)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="name"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        Task<HfUserJobModel> Create(HfUserJobModel data);

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