﻿using Exico.HF.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Exico.HF.Common.DomainModels
{
    public abstract  class HfUserJobModel
    {

        public int Id { get; set; }
 
        [Required(AllowEmptyStrings = false)]
        public string WorkerClassName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string WorkerAssemblyName { get; set; }
        [Required]
        public string TimeZoneId { get; set; }
        [Required, MinLength(1)]
        public string UserId { get; set; }
        [Required, MinLength(1)]
        public string Name { get; set; }
        [Required]
        public JobStatus Status { get;  set; }
        [Required]
        public JobType JobType { get; protected set; }
        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        public string Note { get; set; }
        public int ? WorkDataId { get; set; }        
        public DateTimeOffset? UpdatedOn { get; set; }

        public string HfJobId { get; set; }

        
    }
}
