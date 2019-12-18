using System;
using System.Text.Json.Serialization;
using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
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
        private static int i = 1;

        private readonly IExicoHfDbService _dbService;
        public readonly ILogger<ExicoHfFilter> _logger;

        public ExicoHfFilter(IExicoHfDbService dbService, ILogger<ExicoHfFilter> logger)
        {
            _dbService = dbService;
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

        }

        public void OnStateElection(ElectStateContext context)
        {
            Console.WriteLine($"OnStateElection: {i++}");
            var state = context.CandidateState;
            if (state.Name == FailedState.StateName)
            {
                Console.WriteLine(
                    "OnStateElection(): Job `{0}` has been failed due to an exception `{1}`",
                    context.BackgroundJob.Id,
                    ((FailedState)state).Exception);
            }
            else
            {
                if (context.BackgroundJob != null)
                {
                    var args = GetWorkArguments(context.BackgroundJob.Job);

                    if (state.Name == EnqueuedState.StateName)
                    {
                        if (args.JobType == Common.Enums.JobType.FireAndForget || args.JobType == Common.Enums.JobType.Scheduled)
                        {
                            var result = _dbService.SetHfJobId(args.UserJobId, context.BackgroundJob.Id).Result;
                            _logger.LogInformation($"OnCreated: Set HfJobId to {context.BackgroundJob.Id} for user job {args.UserJobId}");
                        }
                        else if (args.JobType == Common.Enums.JobType.Recurring)
                        {
                            var result = _dbService.SetRecurringLastRunJobId(args.UserJobId, context.BackgroundJob.Id).Result;
                            _logger.LogInformation($"OnCreated: Set Rec Last Run Id to {context.BackgroundJob.Id} for user job {args.UserJobId}");
                        }
                    }
                    else if (state.Name == ProcessingState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Processing);
                    else if(state.Name==ScheduledState.StateName )
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Scheduled);
                    else if (state.Name == DeletedState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Deleted);
                    else if (state.Name == FailedState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Failed);
                    else if (state.Name == SucceededState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Succeeded);


                }
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {

            Console.WriteLine($"OnStateApplied: {i++}");

        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Console.WriteLine($"OnStateUnapplied: {i++}");

        }

        public WorkArguments GetWorkArguments(Job job)
        {
            return (WorkArguments)job.Args[0];

        }
    }
}
