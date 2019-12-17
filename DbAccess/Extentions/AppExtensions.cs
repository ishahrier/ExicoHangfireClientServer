using Exico.HF.DbAccess.Db;
using Exico.HF.DbAccess.Db.Services;
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
            services.AddScoped<MarkerFilter, ExicoHfFilter>();
            services.AddScoped<IManageWork, WorkManager>();


        }

        public static void UseExicoHfExtension (this IApplicationBuilder app)
        {
            GlobalJobFilters.Filters.Add(app.ApplicationServices.GetRequiredService<MarkerFilter>());
        }
    }
}
