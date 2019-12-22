using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Managers
{
    public class WorkManager : IManageWork
    {
        private readonly IExicoHfDbService _dbService;
        private readonly IServiceProvider _provider;

        public WorkManager(IExicoHfDbService dbService,IServiceProvider provider)
        {
            _dbService = dbService;
            _provider = provider;
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
