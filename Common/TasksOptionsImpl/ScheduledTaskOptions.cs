using System;
using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.TasksOptionsImpl {
    public class ScheduledTaskOptions : ABaseOptions, IScheduledTaskOptions {

        public ScheduledTaskOptions():base() => JobType = Interfaces.JobType.Scheduled;

        public DateTime ScheduledAt {
            get => GetOption<DateTime>("ScheduledAt");
            set => SetOption("ScheduledAt", value);
        }

        //todo do catual validation
        public override bool Validate()
        {
            return true;
        }
    }
}