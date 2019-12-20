﻿using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Services;
using Hangfire;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exico.HF.DbAccess.Managers
{
    public class LifeCycleHandler : ILifeCyleHandler
    {
        private readonly IExicoHfDbService _dbService;
        private readonly ILogger<ILifeCyleHandler> _logger;

        public LifeCycleHandler(IExicoHfDbService dbService, ILogger<ILifeCyleHandler> logger)
        {
            this._dbService = dbService;
            this._logger = logger;
        }
        public void HandleOnPerformed(PerformedContext ctx)
        {
            var args = GetWorkArguments(ctx.BackgroundJob.Job);
            if (args.JobType == JobType.Recurring)
            {
                var hfJobId = _dbService.GetHfJobId(args.UserJobId).Result;
                using (var connection = JobStorage.Current.GetConnection())
                {
                    var work = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == hfJobId);
                    if (work != null)
                    {
                        var nextRun = work.NextExecution;
                        _dbService.UpdateRecurringNextRun(args.UserJobId, nextRun.Value).Wait();
                    }
                }
            }
        }

        public void HandleOnStateElection(ElectStateContext context)
        {
            
            var state = context.CandidateState;
            var args = GetWorkArguments(context.BackgroundJob.Job);
            if (state.Name == FailedState.StateName)
            {
                _dbService.UpdateStatus(args.UserJobId, JobStatus.Failed,null).Wait();
            }
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
                    else if (state.Name == DeletedState.StateName)
                    {
                        return;
                    }
                    else if (state.Name == SucceededState.StateName)
                        status = JobStatus.Succeeded;

                    _dbService.UpdateStatus(args.UserJobId, status, context.BackgroundJob.Id);
                }
            }
        }

        protected WorkArguments GetWorkArguments(Job job)
        {
            return (WorkArguments)job.Args[0];
        }
    }
}
