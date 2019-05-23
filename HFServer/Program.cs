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

        protected override async Task Run(IJobCancellationToken cancellationToken)
        {
            var myOptions = (IFireAndForgetTaskOptions)_Options;
            Console.WriteLine(myOptions.GetUserId());
        }
    }
}
