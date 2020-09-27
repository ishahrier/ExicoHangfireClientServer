using Exico.HF.Common.DomainModels;
using Hangfire;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Exico.HF.Common.Worker
{
    public abstract class ABaseWorker:IWorker
    {
        protected readonly ILogger _logger;

        protected ABaseWorker(ILogger logger)
        {
            _logger = logger;
        }

        public  abstract Task<bool> DoWorkAsync(WorkArguments args, IJobCancellationToken token);

    }
}
