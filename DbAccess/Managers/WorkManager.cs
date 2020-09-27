using Exico.HF.Common.DomainModels;
using Exico.HF.DbAccess.Managers.Interfaces;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Exico.HF.Common.Worker;

namespace Exico.HF.DbAccess.Managers
{
    /// <summary>
    /// Responsible for creating worker class instances and tell them to start their work.
    /// </summary>
    public class WorkManager : IManageWork
    {
        private readonly IManageJob _jobManager;
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public WorkManager(IManageJob jobManager, IServiceProvider provider, ILogger<WorkManager> logger)
        {
            _jobManager = jobManager;
            _provider = provider;
            _logger = logger;
        }

        public async Task<bool> ExecuteWorkerAsync(WorkArguments args, IJobCancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInformation("Trying to create worker instance using {@data}", args);
                var wType = Type.GetType(args.GetFullQualifiedWorkerClassName());
                //var wObj = (IWorker)ActivatorUtilities.CreateInstance(_provider, wType);
                var wObj = (IWorker) _provider.GetService(wType);
                _logger.LogInformation("Now executing worker/userJobId {i}", args.UserJobId);
                var ret = await wObj.DoWorkAsync(args, cancellationToken);
                _logger.LogInformation("Finished executing worker/userJobId {id}. Return value is {value}", args.UserJobId, ret);
                return ret;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for worker {@data}", args);
                throw;
            }

        }
    }


}
