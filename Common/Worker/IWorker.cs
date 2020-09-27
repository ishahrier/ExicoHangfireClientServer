using Exico.HF.Common.DomainModels;
using Hangfire;
using System.Threading.Tasks;

namespace Exico.HF.Common.Worker
{
    public interface  IWorker
    {
        Task<bool>  DoWorkAsync(WorkArguments args, IJobCancellationToken token);
    }

}