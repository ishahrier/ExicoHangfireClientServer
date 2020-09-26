using System;
using System.Threading.Tasks;
using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Interfaces;
using Hangfire;

namespace ExampleThirdPartyWorker
{

    public interface IDownloadFromGql : IWorker
    {

    }
    public class GqlDownLoader:IDownloadFromGql
    {
        public GqlDownLoader()
        {
            
        }
        public Task<bool> DoWorkAsync(WorkArguments args, IJobCancellationToken token)
        {
            Console.WriteLine("GqlDownLoader finished Working");
            return Task.FromResult<bool>(true);
        }
    }
}
