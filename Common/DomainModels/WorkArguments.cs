﻿using Exico.HF.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exico.HF.Common.DomainModels
{
 
    public class WorkArguments
    {
        public int UserJobId { get; set; }
        public int? WorkDataId { get; set; }
        public JobType JobType { get; set; }
    }
}
