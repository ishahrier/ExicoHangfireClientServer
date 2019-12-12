using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Exico.HF.Common.Enums;
using Hangfire.Annotations;

namespace Exico.HF.DbAccess.Db.Models
{
    public class HfUserJob
    {
        public uint Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string WorkerClass { get; set; }    
        [Required]
        public string TimeZone { get; set; }
        [Required,MinLength(1)]
        public string UserId { get; set; }
        [Required,MinLength(1)]
        public string Name { get; set; }
        [Required]
        public JobStatus Status { get; set; }
        [Required]
        public JobType JobType { get; set; }
        [Required]
        public DateTimeOffset CreatedOn { get; set; }


        public string Note { get; set; }
        public string HfJobId { get; set; }//this is updated later
        public string JsonOption { get; set; }
        public uint? WorkDataId { get; set; }
        public uint? LastRunHfJobId { get; set; } //for recurring this will be changed every time it is run
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
