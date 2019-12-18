﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.Common.DomainModels
{
    public class HfUserRecurringJobModel: HfUserJobModel
    {
        public HfUserRecurringJobModel()
        {
            this.JobType = Enums.JobType.Recurring;
        }
        public int HfUserRecurringJobModelId { get; set; }
        public string LastHfJobId { get; set; }
        public DateTimeOffset? LastRun { get; set; }
        public DateTimeOffset? NextRun { get; set; }
        public string CronExpression { get; set; }
    }
}
