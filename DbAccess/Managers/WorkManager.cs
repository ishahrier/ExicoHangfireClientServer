using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Managers
{
    /// <summary>
    /// Responsible for creating worker class instances and tell them to start their work.
    /// </summary>
    public class WorkManager : IManageWork
    {
        private readonly IExicoHfDbService _dbService;
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;

        public WorkManager(IExicoHfDbService dbService,IServiceProvider provider,ILogger<WorkManager> logger)
        {
            _dbService = dbService;
            _provider = provider;
            _logger = logger;
        }

 
        public async Task<bool> ExecuteWorkerAsync(WorkArguments args, IJobCancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Trying to create worker instance using {@data}", args);
                var wType = Type.GetType(args.GetFullQualifiedWokerClassName());
                var wObj = (IWorker) ActivatorUtilities.CreateInstance(_provider,wType);
                _logger.LogInformation("Now executing worker {@data}", args);
                var ret = await wObj.DoWorkAsync(args, cancellationToken);
                _logger.LogInformation("Finished executing worker. Return value is {value}", ret);
                return ret ;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception for worker {@data}", args);
                return false;
            }
        }
    }


}
