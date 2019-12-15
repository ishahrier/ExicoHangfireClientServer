using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.Common.DomainModels
{
    public class HfUserRecurringJobModel: HfUserJobModel
    {
        public int HfUserRecurringJobModelId { get; set; }
        public int? LastHfJobId { get; set; }
        public DateTimeOffset? LastRun { get; set; }
        public DateTimeOffset? NextRun { get; set; }
        public string CronExpression { get; set; }
    }
}
