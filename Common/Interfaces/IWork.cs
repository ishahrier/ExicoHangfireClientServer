using Exico.HF.Common.DomainModels;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exico.HF.Common.Interfaces
{
    public interface  IWorker
    {
        Task<bool>  DoWork(WorkArguments args, IJobCancellationToken token);
    }

    public class DownloadAllProducts : IWorker
    {
        private readonly ILogger<DownloadAllProducts> logger;

        public DownloadAllProducts(ILogger<DownloadAllProducts> logger)
        {
            this.logger = logger;
        }
        public async Task<bool> DoWork(WorkArguments args , IJobCancellationToken token)
        {
            var ret = false;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    token.ThrowIfCancellationRequested();
                    this.logger.LogInformation("this is from download all product worker");
                    await Task.Delay( 5000);
                    ret = true;
                }
            }
            catch (Exception)
            {
                logger.LogWarning("Job cancelled");                
            }

            logger.LogInformation("Job ended.");
            return ret;
        }
    }
}