using Exico.HF.Common.DomainModels;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.Common.Interfaces
{
    public interface  IWorker
    {
        string DoWork(WorkArguments args, IJobCancellationToken token);
    }

    public class DownloadAllProducts : IWorker
    {
        private readonly ILogger<DownloadAllProducts> logger;

        public DownloadAllProducts(ILogger<DownloadAllProducts> logger)
        {
            this.logger = logger;
        }
        public string DoWork(WorkArguments args , IJobCancellationToken token)
        {
            this.logger.LogWarning("this is from download all product worker");
            return "this is from download all product worker";
        }
    }
}
