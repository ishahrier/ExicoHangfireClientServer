using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Hangfire.Annotations;

namespace Exico.HF.DbAccess.Db.Models
{
    public class HfUserJob
    {
        public long Id { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        [Required,MinLength(1)]
        public string UserId { get; set; }

        public string HfJobId { get; set; }//this is updated later

        [Required,MinLength(1)]
        public string Name { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public string JsonOption { get; set; }

        [Required, MinLength(1)]
        public string JobType { get; set; }

    }
}
