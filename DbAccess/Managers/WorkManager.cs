using Exico.HF.Common.DomainModels;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Managers
{
    public class WorkManager<T> : IManageWork<T> where T:HfUserJobModel
    {

        public void DoWork(T t, IJobCancellationToken cancellationToken)
        {
            var workerClassName = t.WorkerClass;
            var dataTableId = t.WorkDataId;
            Console.WriteLine($"From worker manager {workerClassName} - {dataTableId}");
        }
    }
}
