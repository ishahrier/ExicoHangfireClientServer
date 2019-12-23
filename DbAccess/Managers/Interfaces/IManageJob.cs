using Exico.HF.Common.DomainModels;
using System.Threading.Tasks;


namespace Exico.HF.DbAccess.Managers
{
    public interface IManageJob
    {
        /// <summary>
        /// Create a user task (FnF, schduled or recurring)
        /// </summary>

        /// <returns></returns>
        Task<T> Create<T>(T t) where T : HfUserJobModel;
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
        Task<bool> RunNow(int id);

        Task<bool> IsAlreadyRunning(int userJob);

        
    }
}