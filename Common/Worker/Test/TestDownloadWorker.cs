using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Exico.HF.Common.DomainModels;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace Exico.HF.Common.Worker.Test
{

    public interface ITestDownloadWorker
    {
    }
    public class TestDownloadWorker:ABaseWorker, ITestDownloadWorker
    {
        public static TestDownloadWorker CreateInstance(ILogger<TestDownloadWorker> logger)
        {
            return new TestDownloadWorker(logger);
        }


        private TestDownloadWorker(ILogger<TestDownloadWorker> logger):base(logger)
        {
        }

        public override async  Task<bool> DoWorkAsync(WorkArguments args , IJobCancellationToken token)
        {
            var ret = false;
            try
            {
                for (var i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    _logger.LogInformation("#{i}.This is from download all product worker. JobType {j}, UserJobId {k}",i,args.JobType.ToString(), args.UserJobId);
                    await Task.Delay( 5000);
                    ret = true;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("Job cancelled JobType {i}, UserJobId {j}", args.JobType.ToString(), args.UserJobId);                
            }

            _logger.LogInformation("Job ended JobType {i}, UserJobId {j}", args.JobType.ToString(), args.UserJobId);
            return ret;
        }
    }
}