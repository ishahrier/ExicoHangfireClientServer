using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.Common.Interfaces
{
    public interface  IWorker
    {
        string DoWork(int workDataId, JobCancellationToken token);
    }

    public class DownloadAllProducts : IWorker
    {
        public string DoWork(int workDataId , JobCancellationToken token)
        {
            return "";
        }
    }
}
