using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Filter.Interfaces
{
    public interface ILifeCycleHandler
    {
        Task<bool> HandleOnStateElection(ElectStateContext ctx);
        Task<bool> HandleOnPerformed(PerformedContext ctx);
    }
}
