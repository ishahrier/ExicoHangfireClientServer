using System;
using Exico.HF.DbAccess.Db.Services;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

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

        private readonly IExicoHFDbService _dbService;
        public ExicoHfFilter(IExicoHFDbService dbService) => _dbService = dbService;

        public void OnCreating(CreatingContext context)
        {
            Console.WriteLine("OnCreating() : Creating a job based on method `{0}`...", context.Job.Method.Name);
        }

        public void OnCreated(CreatedContext context)
        {
            Console.WriteLine(
                "OnCreated() Job that is based on method `{0}` has been created with id `{1}`",
                context.Job.Method.Name,
                context.BackgroundJob?.Id);

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


    }
}
