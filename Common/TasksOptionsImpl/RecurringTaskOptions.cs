﻿using System;
using System.Collections.Generic;
using System.Text;
using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.TasksOptionsImpl
{
    public class RecurringTaskOptions:ABaseTaskOptions, IRecurringTaskOptions
    {
        public RecurringTaskOptions() : base() => SetJobType(JobType.Recurring);

        public void SetCronExpression(string expression) => SetOption("Cron", expression);
        public string GetCronExpression() => GetOption<string>("Cron");
        public override bool Validate()
        {
            return true;
        }
    }
}