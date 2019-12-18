using Exico.HF.Common.DomainModels;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using System;

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
            var i = _dbService.GetHfJobId(args.UserJobId).Result;
            Console.WriteLine($" {args.WorkDataId} -  {args.JobType}");
        }
    }


}
