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
    public interface MarkerFilter
    {

    }
    public class ExicoHfFilter :
       JobFilterAttribute,
       IClientFilter,
       IServerFilter,
       IElectStateFilter,
       IApplyStateFilter,
        MarkerFilter
    {
        private static int i = 1;


        private readonly ILogger<ExicoHfFilter> _logger;
        private readonly ILifeCyleHandler _lifeCycleHandler;

        public ExicoHfFilter(ILifeCyleHandler lifeCYcleHandler, ILogger<ExicoHfFilter> logger)
        {
            _lifeCycleHandler = lifeCYcleHandler;
            _logger = logger;
        }

        public void OnCreating(CreatingContext context)
        {
            Console.WriteLine($"OnCreating: {i++}");
        }

        public void OnCreated(CreatedContext context)
        {
            Console.WriteLine($"Oncreated: {i++}");
        }

        public void OnPerforming(PerformingContext context)
        {
            Console.WriteLine($"OnPerforming: {i++}");
        }

        public void OnPerformed(PerformedContext context)
        {
            Console.WriteLine($"PerformedContext: {i++}");
            var result = _lifeCycleHandler.HandleOnPerformed(context);
        }

        public void OnStateElection(ElectStateContext context)
        {
            Console.WriteLine($"OnStateElection: {i++}");
            var result = _lifeCycleHandler.HandleOnStateElection(context).Result;
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Console.WriteLine($"OnStateApplied: {i++}");
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Console.WriteLine($"OnStateUnapplied: {i++}");
        }

        protected WorkArguments GetWorkArguments(Job job)
        {
            return (WorkArguments)job.Args[0];
        }
    }
}
