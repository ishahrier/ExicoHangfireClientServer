using System;
using System.Collections.Generic;
using System.Text;
using Exico.HF.Common.Interfaces;
using Hangfire;

namespace Exico.HF.DbAccess.Managers
{
    public interface IJobManager
    {
        void Create(IFireAndForgetTaskOptions options);
        void Create(IScheduledTaskOptions options);

    }
}
