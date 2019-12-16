using Exico.HF.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Exico.HF.Common.DomainModels
{
    public class HfUserFireAndForgetJobModel : HfUserJobModel
    {
        public HfUserFireAndForgetJobModel()
        {
            this.JobType = JobType.FireAndForget;
        }
    }
}
