using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Hangfire.Annotations;

namespace Exico.HF.DbAccess.Db.Models
{
    public class HfUserJob
    {
        public string Id { get; set; }
        [Required]
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        [Required]
        [MinLength(1)]
        public string UserId { get; set; }
        [Required]
        [MinLength(1)]
        public string HfJobId { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }

    }
}
