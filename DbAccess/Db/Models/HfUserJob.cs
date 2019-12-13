using Exico.HF.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Exico.HF.DbAccess.Db.Models
{
    public class HfUserJob
    {
        public int Id { get; set; }
        [Required, MinLength(1)] 
        public string Name { get; set; }
        [Required] 
        public JobStatus Status { get; set; }
        [Required]
        public JobType JobType { get; set; }
        [Required(AllowEmptyStrings = false), MinLength(1),]
        public string UserId { get; set; }
        public string HfJobId { get; set; }//this is updated later        
        public int? WorkDataId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string WorkerClass { get; set; }
        public string Note { get; set; }
        [Required]
        public string TimeZone { get; set; }
        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }

    public class HfUserRecurringJob
    {
        public int Id { get; set; }
        public int HfUserJobId { get; set; }
        public int? LastHfJobId { get; set; }
        public DateTimeOffset? LastRun { get; set; }
        public DateTimeOffset? NextRun { get; set; }
        public HfUserJob HfUserJob { get; set; }
    }

    public class HfUserScheduledJob
    {
        public int Id { get; set; }
        public int HfUserJobId { get; set; }
        public DateTimeOffset ScheduledAt { get; set; }
        public string CronExpression { get; set; }
        public HfUserJob HfUserJob { get; set; }

    }
}
