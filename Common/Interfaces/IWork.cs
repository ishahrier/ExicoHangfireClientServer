using Exico.HF.Common.DomainModels;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Exico.HF.Common.Interfaces
{
    public interface  IWorker
    {
        void  DoWork(WorkArguments args, IJobCancellationToken token);
    }

    public class DownloadAllProducts : IWorker
    {
        private readonly ILogger<DownloadAllProducts> logger;

        public DownloadAllProducts(ILogger<DownloadAllProducts> logger)
        {
            this.logger = logger;
        }
        public void DoWork(WorkArguments args , IJobCancellationToken token)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    token.ThrowIfCancellationRequested();
                    this.logger.LogInformation("this is from download all product worker");
                    Thread.Sleep(5000);
                }
            }
            catch (Exception)
            {
                logger.LogWarning("Job cancelled");                
            }

            logger.LogInformation("Job ended.");
        }
    }
}