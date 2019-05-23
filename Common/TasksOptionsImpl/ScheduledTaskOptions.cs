using System;
using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.TasksOptionsImpl
{
    public class ScheduledTaskOptions : ABaseOptions, IScheduledTaskOptions
    {

        public ScheduledTaskOptions() : base() => SetJobType(JobType.Scheduled);

        public DateTime GetScheduledAt() => GetOption<DateTime>("ScheduledAt");
        public void SetScheduledAt(DateTime dt) => SetOption("ScheduledAt", new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Unspecified));

        //TODO do catual validation
        public override bool Validate()
        {
            return true;
        }
    }
}