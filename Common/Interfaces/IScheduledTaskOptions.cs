using System;

namespace Exico.HF.Common.Interfaces {
    public interface IScheduledTaskOptions : IBaseTaskOptions {

        DateTime ScheduledAt { get; set; }
    }
}