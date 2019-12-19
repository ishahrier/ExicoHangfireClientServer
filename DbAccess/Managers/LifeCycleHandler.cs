using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Services;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Managers
{
    public class LifeCycleHandler : ILifeCyleHandler
    {
        private readonly IExicoHfDbService _dbService;

        public LifeCycleHandler(IExicoHfDbService dbService)
        {
            this._dbService = dbService;
        }
        public void HandleOnPerformed(PerformedContext ctx)
        {
            //var args = GetWorkArguments(context.BackgroundJob.Job);
            //if (args.JobType == JobType.Recurring)
            //{
            //    var hfJobId = _dbService.GetHfJobId(args.UserJobId).Result;
            //    using (var connection = JobStorage.Current.GetConnection())
            //    {
            //        var work = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == hfJobId);
            //        if (work != null)
            //        {
            //            var nextRun = work.NextExecution;                                                
            //            _dbService.UpdateRecurringNextRun(args.UserJobId, nextRun.Value).Wait();
            //        }                    
            //    }
            //}
        }

        public void HandleOnStateElection(ElectStateContext ctx)
        {
            
            var state = ctx.CandidateState;
            var args = GetWorkArguments(ctx.BackgroundJob.Job);
            if (state.Name == FailedState.StateName)
            {
                _dbService.UpdateStatus(args.UserJobId, JobStatus.Failed).Wait();

            }
            else
            {
                if (context.BackgroundJob != null)
                {

                    if (state.Name == EnqueuedState.StateName)
                    {
                        if (args.JobType == JobType.FireAndForget || args.JobType == JobType.Scheduled)
                        {
                            var result = _dbService.SetHfJobId(args.UserJobId, context.BackgroundJob.Id).Result;
                            _logger.LogInformation($"OnCreated: Set HfJobId to {context.BackgroundJob.Id} for user job {args.UserJobId}");
                        }
                        else if (args.JobType == JobType.Recurring)
                        {
                            var result = _dbService.SetRecurringLastRunJobId(args.UserJobId, context.BackgroundJob.Id).Result;
                            _logger.LogInformation($"OnCreated: Set Rec Last Run Id to {context.BackgroundJob.Id} for user job {args.UserJobId}");
                        }
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Enqueued).Wait();
                    }
                    else if (state.Name == ProcessingState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Processing).Wait();
                    else if (state.Name == ScheduledState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Scheduled).Wait();
                    else if (state.Name == DeletedState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Deleted).Wait();
                    else if (state.Name == FailedState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Failed).Wait();
                    else if (state.Name == SucceededState.StateName)
                        _dbService.UpdateStatus(args.UserJobId, JobStatus.Succeeded).Wait();


                }
            }
        }

        protected WorkArguments GetWorkArguments(Job job)
        {
            return (WorkArguments)job.Args[0];

        }
    }
}
