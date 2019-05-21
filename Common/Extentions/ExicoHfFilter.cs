using System;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace Exico.HF.Common.Extentions
{
    public class
       ExicoHfFilter :
       JobFilterAttribute,
       IClientFilter,
       IServerFilter,
       IElectStateFilter,
       IApplyStateFilter
    {


        public void OnCreating(CreatingContext context)
        {
            Console.WriteLine("Creating a job based on method `{0}`...", context.Job.Method.Name);
        }

        public void OnCreated(CreatedContext context)
        {
            Console.WriteLine(
                "Job that is based on method `{0}` has been created with id `{1}`",
                context.Job.Method.Name,
                context.BackgroundJob?.Id);

        }

        public void OnPerforming(PerformingContext context)
        {
            Console.WriteLine("Starting to perform job `{0}`", context.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext context)
        {
            Console.WriteLine("Job `{0}` has been performed", context.BackgroundJob.Id);
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                Console.WriteLine(
                    "Job `{0}` has been failed due to an exception `{1}`",
                    context.BackgroundJob.Id,
                    failedState.Exception);
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Console.WriteLine(
                "Job `{0}` state was changed from `{1}` to `{2}`",
                context.BackgroundJob.Id,
                context.OldStateName,
                context.NewState.Name);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Console.WriteLine(
                "Job `{0}` state `{1}` was unapplied.",
                context.BackgroundJob.Id,
                context.OldStateName);
        }


    }
}
