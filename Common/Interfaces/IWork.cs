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
        Task<bool>  DoWorkAsync(WorkArguments args, IJobCancellationToken token);
    }

    public class DownloadAllProducts : IWorker
    {
        private readonly ILogger<DownloadAllProducts> logger;

        public DownloadAllProducts(ILogger<DownloadAllProducts> logger)
        {
            this.logger = logger;
        }
        public async Task<bool> DoWorkAsync(WorkArguments args , IJobCancellationToken token)
        {
            var ret = false;
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    this.logger.LogInformation("#{i}.This is from download all product worker. JobtType {j}, UserJobId {k}",i,args.JobType.ToString(), args.UserJobId);
                    await Task.Delay( 5000);
                    ret = true;
                }
            }
            catch (Exception)
            {
                logger.LogWarning("Job cancelled JobtType {i}, UserJobId {j}", args.JobType.ToString(), args.UserJobId);                
            }

            logger.LogInformation("Job ended JobtType {i}, UserJobId {j}", args.JobType.ToString(), args.UserJobId);
            return ret;
        }
    }
}