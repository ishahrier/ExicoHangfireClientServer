using Exico.HF.Common.Extentions;
using Exico.HF.Common.Interfaces;
using Exico.HF.Common.TasksOptionsImpl;
using Exico.HF.DbAccess.Db;
using Exico.HF.DbAccess.Db.Services;
using Exico.HF.DbAccess.Managers;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Exico.HF.DbAccess.Extentions
{
    public static class AddService
    {
        public static void AddExicoHfDbServices(this IServiceCollection services, string conString)
        {

            services.AddScoped<IJobManager, JobManager>();
            services.AddDbContext<ExicoHfDbContext>(x => x.UseSqlServer(conString));
            services.AddScoped<IExicoHFDbService, ExicoHFDbService>();
            GlobalJobFilters.Filters.Add(new ExicoHfFilter());
        }
    }
}
