using Exico.HF.Common.DomainModels;
using Exico.HF.DbAccess.Extentions;
using Exico.HF.DbAccess.Managers;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Filter
{
    public abstract class ABaseExicoHfFilter : JobFilterAttribute, IExicoHfFilter
    {
        private ABaseExicoHfFilter() { }
        protected readonly ILogger<DefaultExicoHfFilter> _logger;
        protected readonly ILifeCyleHandler _lifeCycleHandler;
        public ABaseExicoHfFilter(ILifeCyleHandler lifeCYcleHandler, ILogger<DefaultExicoHfFilter> logger)
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

}
