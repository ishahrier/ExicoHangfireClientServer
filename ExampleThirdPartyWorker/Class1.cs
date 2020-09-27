using System;
using System.Threading.Tasks;
using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Worker;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace ExampleThirdPartyWorker
{

    public interface IDownloadFromGql 
    {

    }
    public class GqlDownLoader:ABaseWorker,IDownloadFromGql
    {

        public override  async Task<bool> DoWorkAsync(WorkArguments args, IJobCancellationToken token)
        {
            Console.WriteLine("GqlDownLoader finished Working");
            return await Task.FromResult<bool>(true);
        }

        public GqlDownLoader(ILogger<GqlDownLoader> logger) : base(logger)
        {
        }
    }
}
