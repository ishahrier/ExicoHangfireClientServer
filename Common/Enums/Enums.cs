using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.Common.Enums
{
    public enum JobType
    {
        FireAndForget=1,
        Scheduled,
        Recurring,
    }

    public enum JobStatus
    {
        None=1,
        Enqueued,
        Processing,
        Cancelled,
        Deleted,
        Scheduled,
        Failed,
        Succeeded
    }
 
}
