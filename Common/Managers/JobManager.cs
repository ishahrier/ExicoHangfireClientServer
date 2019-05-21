using System;
using System.Collections.Generic;
using System.Text;
using Exico.HF.Common.Interfaces;
using Hangfire;

namespace Exico.HF.Common.Managers
{
    public class JobManager
    {
        public void CreateFireAndForgetJob(IFireAndForgetTaskOptions options, IBackgroundJobClient hfClient)
        {
            hfClient.Enqueue<IFireAndForgetTask>(x => x.Run(options.ToJson(), JobCancellationToken.Null));
        }
    }
}
