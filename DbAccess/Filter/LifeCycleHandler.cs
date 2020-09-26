using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.DbAccess.Filter.Interfaces;

namespace Exico.HF.DbAccess.Filter
{

    public class LifeCycleHandler : ILifeCycleHandler
    {
        private readonly IExicoHfDbService _dbService;
        private readonly ILogger<ILifeCycleHandler> _logger;

        public LifeCycleHandler(IExicoHfDbService dbService, ILogger<ILifeCycleHandler> logger)
        {
            this._dbService = dbService;
            this._logger = logger;
        }
        public async Task<bool> HandleOnPerformed(PerformedContext ctx)
        {
            var args = GetWorkArguments(ctx.BackgroundJob.Job);
            if (args.JobType == JobType.Recurring)
            {
                var hfJobId = await _dbService.GetRecurringInitialHfJobId(args.UserJobId);
                using (var connection = JobStorage.Current.GetConnection())
                {
                    var work = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == hfJobId.ToString());
                    if (work != null)
                    {
                        var nextRun = work.NextExecution;
                        return await _dbService.UpdateRecurringNextRun(args.UserJobId, nextRun.Value);
                    }
                }
            }
            return true;
        }

        public async Task<bool> HandleOnStateElection(ElectStateContext context)
        {
            try
            {
                var state = context.CandidateState;
                var args = GetWorkArguments(context.BackgroundJob.Job);
                if (state.Name == FailedState.StateName)
                    return await _dbService.UpdateStatusBgJobId(args.UserJobId, JobStatus.Failed, context.BackgroundJob?.Id);
                else
                {
                    if (context.BackgroundJob != null)
                    {
                        JobStatus status = JobStatus.None;
                        if (state.Name == EnqueuedState.StateName)
                            status = JobStatus.Enqueued;
                        else if (state.Name == ProcessingState.StateName)
                            status = JobStatus.Processing;
                        else if (state.Name == ScheduledState.StateName)
                            status = JobStatus.Scheduled;
                        else if (state.Name == SucceededState.StateName)
                            status = JobStatus.Succeeded;
                        else if (state.Name == DeletedState.StateName)
                            status = JobStatus.Cancelled;

                        return await _dbService.UpdateStatusBgJobId(args.UserJobId, status, context.BackgroundJob.Id);
                    }
                }
                return false;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        protected WorkArguments GetWorkArguments(Job job)
        {
            return (WorkArguments)job.Args[0];
        }
    }
}
