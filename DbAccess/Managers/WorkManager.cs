using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;
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
        //public void DoWork(T t, IJobCancellationToken cancellationToken)
        //{
        //    var workerClassName = t.WorkerClass;
        //    var dataTableId = t.WorkDataId;
        //    Console.WriteLine($"{t.JobType.ToString() } | UserHfJobId {t.Id} | hfId {t.HfJobId} : [started]");
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        try
        //        {
        //            cancellationToken.ThrowIfCancellationRequested();
        //            Console.WriteLine($"#{i}.Worker class name:  {workerClassName} | word data table:{dataTableId}");
        //            Thread.Sleep(5000);
        //        }
        //        catch (OperationCanceledException ex)
        //        {

        //            Console.WriteLine($"{t.JobType.ToString() } | UserHfJobId {t.Id} | hfId {t.HfJobId} : [cancelled]");

        //            break;
        //        }

        //    }

        //    Console.WriteLine($"{t.JobType.ToString() } | UserHfJobId {t.Id} | hfId {t.HfJobId} : [ended]");

        //}

        public void DoWork(WorkArguments args, IJobCancellationToken cancellationToken)
        {
            var i = _dbService.GetHfJobId(args.UserJobId).Result;
            Console.WriteLine($"{args.WorkDataId} -  {args.JobType}");
        }
    }


}
