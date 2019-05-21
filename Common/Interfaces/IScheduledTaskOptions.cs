using System;

namespace Exico.HF.Common.Interfaces {
    public interface IScheduledTaskOptions : IBaseTaskOptions {

        TimeSpan ScheduledAt { get; set; }
    }
}