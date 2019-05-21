using Exico.HF.Common.Interfaces;
using Hangfire;

namespace Exico.HF.DbAccess.Managers
{
    public class JobManager
    {
        public void CreateFireAndForgetJob(IFireAndForgetTaskOptions options, IBackgroundJobClient hfClient)
        {
            hfClient.Enqueue<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null));
        }
    }
}
