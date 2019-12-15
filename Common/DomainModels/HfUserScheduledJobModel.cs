using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.Common.DomainModels
{
    public class HfUserScheduledJobModel:HfUserJobModel
    {
        public DateTimeOffset ScheduledAt { get; set; }
    }
}
