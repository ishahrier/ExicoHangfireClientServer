using System;

namespace Exico.HF.Common.Interfaces {
    public interface IScheduledTaskOptions : IBaseTaskOptions {        
        DateTime GetScheduledAt();
        void SetScheduledAt(DateTime dt);
    }
}