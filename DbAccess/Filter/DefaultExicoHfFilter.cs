using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.Logging;

namespace Exico.HF.DbAccess.Filter
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
