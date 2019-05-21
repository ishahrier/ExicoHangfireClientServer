using System;
using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.TasksOptionsImpl {
    public class ScheduledTaskOptions : ABaseOptions, IScheduledTaskOptions {

        public ScheduledTaskOptions():base() => JobType = Interfaces.JobType.Scheduled;

        public TimeSpan ScheduledAt {
            get => GetOption<TimeSpan>("ScheduledAt");
            set => SetOption("ScheduledAt", value);
        }
    }
}