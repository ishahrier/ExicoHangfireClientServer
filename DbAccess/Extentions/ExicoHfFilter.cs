using Exico.HF.Common.DomainModels;
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
    public interface IExicoFilter :
       IClientFilter,
       IServerFilter,
       IElectStateFilter,
       IApplyStateFilter
    {
        void OnPerformed(PerformedContext context);
    }

    public abstract class ABaseHfFilter : JobFilterAttribute, IExicoFilter
    {
        private ABaseHfFilter() { }
        protected readonly ILogger<ExicoHfFilter> _logger;
        protected readonly ILifeCyleHandler _lifeCycleHandler;
        public ABaseHfFilter(ILifeCyleHandler lifeCYcleHandler, ILogger<ExicoHfFilter> logger)
        {
            _lifeCycleHandler = lifeCYcleHandler;
            _logger = logger;
        }
        protected WorkArguments GetWorkArguments(Job job) => (WorkArguments)job.Args[0];

        public virtual void OnCreating(CreatingContext filterContext) { }

        public virtual void OnCreated(CreatedContext filterContext) { }

        public virtual void OnPerforming(PerformingContext filterContext) { }

        public virtual void OnPerformed(PerformedContext filterContext) { }

        public virtual void OnStateElection(ElectStateContext context) { }

        public virtual void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction) { }

        public virtual void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction) { }
    }

    public class ExicoHfFilter : ABaseHfFilter
    {
        public ExicoHfFilter(ILifeCyleHandler lifeCYcleHandler, ILogger<ExicoHfFilter> logger) :
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
