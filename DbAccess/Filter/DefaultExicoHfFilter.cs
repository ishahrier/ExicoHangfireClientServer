using Exico.HF.Common.DomainModels;
using Exico.HF.DbAccess.Filter;
using Exico.HF.DbAccess.Managers;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Extentions
{


    public class DefaultExicoHfFilter : ABaseExicoHfFilter
    {
        public DefaultExicoHfFilter(ILifeCyleHandler lifeCYcleHandler, ILogger<DefaultExicoHfFilter> logger) :
            base(lifeCYcleHandler, logger)
        { }

        public override void OnPerformed(PerformedContext context)
        {
            var result = _lifeCycleHandler.HandleOnPerformed(context).Result;
        }

        public override void OnStateElection(ElectStateContext context)
        {
            var result = _lifeCycleHandler.HandleOnStateElection(context).Result;
        }
    }
}
