using Exico.HF.DbAccess.Db;
using Exico.HF.DbAccess.Db.Services;
using Exico.HF.DbAccess.Filter;
using Exico.HF.DbAccess.Managers;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Exico.HF.DbAccess.Extentions
{
    public static class AppExtensions
    {
        public static void AddExicoHfExtension(this IServiceCollection services, string conString)
        {

            services.AddDbContext<ExicoHfDbContext>(x => x.UseSqlServer(conString));
            services.AddScoped<IManageJob, JobManager>();
            services.AddScoped<IExicoHfDbService, ExicoHfDbService>();            
            services.AddScoped<IExicoHfFilter, DefaultExicoHfFilter>();
            services.AddScoped<IManageWork, WorkManager>();
            services.AddScoped<IGenerateDbContext, GenerateDbContext>();
            services.AddScoped<ILifeCyleHandler, LifeCycleHandler>();

        }

        public static void UseExicoHfExtension (this IApplicationBuilder app)
        {
            GlobalJobFilters.Filters.Add(app.ApplicationServices.GetRequiredService<IExicoHfFilter>());
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
        }
    }
}
