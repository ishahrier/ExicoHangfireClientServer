﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Exico.HF.Common.DomainModels
{
    public class HfUserScheduledJobModel:HfUserJobModel
    {
        public HfUserScheduledJobModel()
        {
            this.JobType = Enums.JobType.Scheduled;
        }
        public int HfUserScheduledJobModelId { get; set; }
        [Required]
        public DateTimeOffset ScheduledAt { get; set; }
    }
}
