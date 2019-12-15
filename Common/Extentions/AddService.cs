using Exico.HF.Common.Interfaces;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Exico.HF.Common.Extentions
{
    public static class AddService
    {
        public static void AddExicoHfExtension(this IServiceCollection services )
        {
            //services.AddScoped<IFireAndForgetTaskOptions, FireAndForgetTaskOptions>();
            //services.AddScoped<IScheduledTaskOptions, ScheduledTaskOptions>();
            //services.AddScoped<IRecurringTaskOptions, RecurringTaskOptions>();
            

        }
    }
}
