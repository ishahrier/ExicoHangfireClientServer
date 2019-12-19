using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Managers
{
    public interface ILifeCyleHandler
    {
        void HandleOnStateElection(ElectStateContext ctx);
        void HandleOnPerformed(PerformedContext ctx);
    }
}
