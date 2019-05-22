using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using Hangfire;
 

namespace Exico.HF.DbAccess.Managers
{
    public interface IJobManager
    {
        Task<HfUserJob>  Create(IFireAndForgetTaskOptions options, string name, string note);
        Task<HfUserJob> Create(IScheduledTaskOptions options, string name, string note);
    }
}
