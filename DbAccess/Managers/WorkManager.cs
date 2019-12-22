using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace Exico.HF.DbAccess.Managers
{
    public class WorkManager : IManageWork
    {
        private readonly IExicoHfDbService _dbService;
        private readonly IServiceProvider provider;

        public WorkManager(IExicoHfDbService dbService,IServiceProvider provider)
        {
            _dbService = dbService;
            this.provider = provider;
        }

        private T GetObject<T> (T  type) where T: Type
        {
            return ActivatorUtilities.CreateInstance<T>(this.provider);
        }
        public void ExecWorker(WorkArguments args, IJobCancellationToken cancellationToken)
        {
            try
            {
                var t = Type.GetType(args.WorkerClass);


                var obj = GetObject(t);
                
                var i = _dbService.GetHfJobId(args.UserJobId).Result;
                for (int j = 0; j < 10; j++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($" {args.WorkDataId} -  {args.JobType}");
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }


}
