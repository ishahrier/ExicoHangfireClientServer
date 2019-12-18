using System;
using System.Text.Json.Serialization;
using Exico.HF.Common.DomainModels;
using Exico.HF.DbAccess.Db.Services;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Exico.HF.DbAccess.Extentions
{
    public interface MarkerFilter
    {
        WorkArguments GetWorkArguments(Job job);
    }
    public class ExicoHfFilter :
       JobFilterAttribute,
       IClientFilter,
       IServerFilter,
       IElectStateFilter,
       IApplyStateFilter,
        MarkerFilter
    {


        private readonly IExicoHfDbService _dbService;
        public readonly ILogger<ExicoHfFilter> _logger;

        public ExicoHfFilter(IExicoHfDbService dbService,ILogger<ExicoHfFilter> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        public void OnCreating(CreatingContext context)
        {
            Console.WriteLine("OnCreating() : Creating a job based on method `{0}`...", context.Job.Method.Name);
        }

        public void OnCreated(CreatedContext context)
        {

        }

        public void OnPerforming(PerformingContext context)
        {
            Console.WriteLine("OnPerforming() Starting to perform job `{0}`", context.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext context)
        {
            Console.WriteLine("OnPerformed() Job `{0}` has been performed", context.BackgroundJob.Id);
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                Console.WriteLine(
                    "OnStateElection(): Job `{0}` has been failed due to an exception `{1}`",
                    context.BackgroundJob.Id,
                    failedState.Exception);
            }
            else
            {
                if (context.BackgroundJob != null)
                {
                    var args = GetWorkArguments(context.BackgroundJob.Job);
                    var result = _dbService.SetHfJobId(args.UserJobId, context.BackgroundJob.Id).Result;
                    _logger.LogInformation($"OnCreated: Set HfJobId to {context.BackgroundJob.Id} for user job {args.UserJobId}");
                }

                Console.WriteLine("OnStateElection(): state selection happened.");
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            
            Console.WriteLine(
                "OnStateApplied(): Job `{0}` state was changed from `{1}` to `{2}`",
                context.BackgroundJob.Id,
                context.OldStateName,
                context.NewState.Name);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Console.WriteLine(
                "OnStateUnapplied() : Job `{0}` state `{1}` was unapplied.",
                context.BackgroundJob.Id,
                context.OldStateName);
        }

        public WorkArguments GetWorkArguments(Job job)
        {
            return (WorkArguments) job.Args[0];
            
        }
    }
}
