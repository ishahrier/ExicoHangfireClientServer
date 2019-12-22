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

        public WorkManager(IExicoHfDbService dbService,IServiceProvider provider,ILogger logger)
        {
            _dbService = dbService;
            _provider = provider;
            _logger = logger;
        }

 
        public async Task<bool> ExecuteWorker(WorkArguments args, IJobCancellationToken cancellationToken)
        {
            try
            {
                var wType = Type.GetType(args.GetFullQualifiedWokerClassName());
                var wObj = (IWorker) ActivatorUtilities.CreateInstance(_provider,wType);
                var ret = await wObj.DoWork(args, cancellationToken);
                return ret ;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


}
