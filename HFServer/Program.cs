using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HFServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class MyFnFJob : ABaseFireAndForgetTask
    {
        public MyFnFJob(IFireAndForgetTaskOptions options) : base(options)
        {
        }

        public override void UpdateTaskStatus()
        {

        }

        protected override async Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options)
        {
            var myOptions = (IFireAndForgetTaskOptions)options;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("This is a fire and forget task");
            Console.WriteLine("User id is : " + myOptions.GetUserId());
            Console.ResetColor();
        }
    }

    public class MyScheduledJob : ABaseScheduledtTask
    {
        public MyScheduledJob(IScheduledTaskOptions options) : base(options)
        {
        }

        public override void UpdateTaskStatus()
        {

        }

        protected override async Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options)
        {
            var myOptions = (IScheduledTaskOptions)options;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("This is a scheudled task");
            Console.WriteLine("User id is : " + myOptions.GetUserId());
            Console.ResetColor();
        }
    }
    public class MyRecurringJob : ABaseRecurringTask
    {
        public MyRecurringJob(IRecurringTaskOptions options) : base(options)
        {
        }

        public override void UpdateTaskStatus()
        {

        }

        protected override async Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options)
        {
            var myOptions = (IRecurringTaskOptions)options;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("This is a Recurring task");
            Console.WriteLine("User id is : " + myOptions.GetUserId());
            Console.ResetColor();
        }
    }
}
