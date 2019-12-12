using System.Threading.Tasks;
using Hangfire;

namespace Exico.HF.Common.Interfaces {
    public interface IBaseTask {
        Task Run (string jsonOptionsString, IJobCancellationToken cancellationToken);
        void UpdateTaskStatus();

    }
}