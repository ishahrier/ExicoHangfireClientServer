using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
            .UseDefaultServiceProvider(options => options.ValidateScopes = false)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }

    //public class MyFnFJob : ABaseFireAndForgetTask
    //{
    //    public MyFnFJob(IFireAndForgetTaskOptions options) : base(options)
    //    {
    //    }

    //    public override void UpdateTaskStatus()
    //    {

    //    }

    //    protected override async Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options)
    //    {
    //        var myOptions = (IFireAndForgetTaskOptions)options;
    //        Console.ForegroundColor = ConsoleColor.Cyan;

    //        Console.ResetColor();
    //        bool cancelled = false;
    //        for (int i = 1; i <= 10 && !cancelled; i++)
    //        {
    //            Console.WriteLine($"User side task id : {options.GetUserTaskId()} | User id: {options.GetUserId()} ");
    //            Thread.Sleep(2000);
    //            try
    //            {
    //                cancellationToken.ThrowIfCancellationRequested();

    //            }
    //            catch (OperationCanceledException ex)
    //            {
    //                Console.WriteLine($"Cancellation requested for job id {options.GetUserTaskId()}. quiting job...");
    //                cancelled = true;
    //            }

    //        }
    //        if (!cancelled)
    //            Console.WriteLine("Job ended normally.");
    //    }
    //}

    //public class MyScheduledJob : ABaseScheduledtTask
    //{
    //    public MyScheduledJob(IScheduledTaskOptions options) : base(options)
    //    {
    //    }

    //    public override void UpdateTaskStatus()
    //    {

    //    }

    //    protected override async Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options)
    //    {
    //        var myOptions = (IScheduledTaskOptions)options;
    //        Console.ForegroundColor = ConsoleColor.Cyan;
    //        Console.WriteLine("This is a scheudled task");
    //        Console.WriteLine("User id is : " + myOptions.GetUserId());
    //        Console.ResetColor();
    //    }
    //}
    //public class MyRecurringJob : ABaseRecurringTask
    //{
    //    public MyRecurringJob(IRecurringTaskOptions options) : base(options)
    //    {
    //    }

    //    public override void UpdateTaskStatus()
    //    {

    //    }

    //    protected override async Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options)
    //    {
    //        var myOptions = (IRecurringTaskOptions)options;
    //        Console.ForegroundColor = ConsoleColor.Cyan;

    //        Console.ResetColor();
    //        bool cancelled = false;
    //        for (int i = 1; i <= 10 && !cancelled; i++)
    //        {
    //            Console.WriteLine($"User side task id : {options.GetUserTaskId()} | User id: {options.GetUserId()} ");
    //            Thread.Sleep(2000);
    //            try
    //            {
    //                cancellationToken.ThrowIfCancellationRequested();

    //            }
    //            catch (OperationCanceledException ex)
    //            {
    //                Console.WriteLine($"Cancellation requested for job id {options.GetUserTaskId()}. quiting job...");
    //                cancelled = true;
    //            }

    //        }
    //        if (!cancelled)
    //            Console.WriteLine("Job ended normally.");
    //    }
    //}
}
