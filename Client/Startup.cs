using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_110)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient bgJob, IRecurringJobManager recJobf)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                string jobId = "";
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("welcome");
                });
                endpoints.MapGet("/create", async context =>
                {
                    
                    //jobId = bgJob.Enqueue(() => new LongTask().RunLongTask(JobCancellationToken.Null));
                   // recJobf.AddOrUpdate("id",Job.FromExpression(()=>Console.WriteLine("Asd")), Cron.Daily());
                    

                    await context.Response.WriteAsync("create");
                });
                endpoints.MapGet("/cancel", async context =>
                {
                    bgJob.Delete(jobId);
                    await context.Response.WriteAsync("cancelled");
                });
            });
        }


    }
}
