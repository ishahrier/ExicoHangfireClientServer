using Exico.HF.Common.Extentions;
using Exico.HF.Common.Interfaces;
using Exico.HF.Common.TasksOptionsImpl;
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

            services.AddScoped<IJobManager, JobManager>();
            services.AddDbContext<ExicoHfDbContext>(x => x.UseSqlServer(conString));
            services.AddScoped<IExicoHFDbService, ExicoHFDbService>();
            services.AddScoped<MarkerFilter, ExicoHfFilter>();

        }

        public static void UseExicoHfExtension (this IApplicationBuilder app)
        {
            //var dbService = app.ApplicationServices.GetService<IExicoHFDbService>();
          //  var d = app.ApplicationServices.GetService<IExicoHFDbService>();
          //  GlobalJobFilters.Filters.Add(app.ApplicationServices.GetRequiredService<MarkerFilter>());
        }
    }
}
