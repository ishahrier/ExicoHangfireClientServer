using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.DbAccess.Db.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client
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


        public static void  TimeConversion()
        {
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                // Console.WriteLine(tz.Id + " " + tz);
            }

            var aTime = new DateTime(2019, 5, 22, 11, 00, 00, DateTimeKind.Unspecified);
            var utc = TimeZoneInfo.ConvertTimeToUtc(aTime, TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time"));
            var local = TimeZoneInfo.ConvertTime(utc, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            Console.WriteLine(local.ToString("F"));
            Console.ReadKey();
        }
    }
}
