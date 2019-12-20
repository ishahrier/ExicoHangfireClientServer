using Exico.HF.Common.DomainModels;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using System;
using System.Threading;

namespace Exico.HF.DbAccess.Managers
{
    public class WorkManager : IManageWork
    {
        private IExicoHfDbService _dbService;

        public WorkManager(IExicoHfDbService dbService)
        {
            _dbService = dbService;
        }

        public void DoWork(WorkArguments args, IJobCancellationToken cancellationToken)
        {
            try
            {
                
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
