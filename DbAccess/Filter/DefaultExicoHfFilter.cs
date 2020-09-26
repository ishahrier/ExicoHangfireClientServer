using Exico.HF.DbAccess.Managers;
using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.Logging;
using System;
using Exico.HF.DbAccess.Filter.Interfaces;
using Exico.HF.DbAccess.Managers.Interfaces;

namespace Exico.HF.DbAccess.Filter
{


    public class DefaultExicoHfFilter : ABaseExicoHfFilter
    {
        public DefaultExicoHfFilter(ILifeCycleHandler lifeCYcleHandler,IManageJob manager, ILogger<DefaultExicoHfFilter> logger) :
            base(lifeCYcleHandler, manager, logger)
        { }

        public override void OnCreating(CreatingContext filterContext)
        {
            var args = GetWorkArguments(filterContext.Job);
            var isRunning = manager.IsAlreadyRunning(args.UserJobId).Result;
            if (isRunning)
            {
                _logger.LogWarning("Skipping background job creation for {@args}.",args);
                filterContext.Canceled = true;
            }
        }
        public override void OnPerformed(PerformedContext context)
        {
            
            try
            {
                var result = _lifeCycleHandler.HandleOnPerformed(context).Result;
                if (!result) _logger.LogError("Could not handle OnPerformed() for {@arg}", GetWorkArguments(context.BackgroundJob?.Job));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling OnPerformed() for {@arg} for {@arg}", GetWorkArguments(context.BackgroundJob?.Job));
            }

        }

        public override void OnStateElection(ElectStateContext context)
        {
            
            try
            {                
                var result = _lifeCycleHandler.HandleOnStateElection(context).Result;
                if (!result) _logger.LogError("Could not handle onStateElection() for {@arg}", GetWorkArguments(context.BackgroundJob?.Job));
            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling onStateElection() for {@arg}", GetWorkArguments(context.BackgroundJob?.Job));
            } 
            
        }
    }
}
