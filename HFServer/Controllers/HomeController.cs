using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.DbAccess.Managers;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using HFServer.Models;
using Exico.HF.DbAccess.Extentions;
using Exico.HF.DbAccess.Db.Services;
using Exico.HF.Common.DomainModels;

namespace HFServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IManageJob _jm;
        private readonly IExicoHfDbService _service;

        public HomeController(IManageJob jm, IExicoHfDbService service)
        {
            _jm = jm;
            _service = service;
   
        }
        public IActionResult Index()
        {
           var data =  _service.Create(new HfUserRecurringJobModel()
            {
                Name = "Tst Fnf",
                Note = "Test Note",
                UserId = "1111",
                WorkerClass = "The Fnf Worker",
                JobType = Exico.HF.Common.Enums.JobType.FireAndForget,
                TimeZoneId = "Central Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                CronExpression = "cron"               
                
            }).Result;
            return View();
        }

        //public async Task<IActionResult> CreateFnF()
        //{
        //    var options2= new FireAndForgetTaskOptions();
        //    options2.SetTimeZoneId("Central Standard Time");
        //    options2.SetUserId("1000");
        //    await _jm.Create(options2, "Fnf", "Fnf note");
        //    return View("Index");
        //}
        
        //public async Task<ActionResult> CreateScheduled(int minAfter=1)
        //{

        //    var options = new ScheduledTaskOptions();
        //    options.SetTimeZoneId("Central Standard Time");
        //    var now = DateTime.Now.AddSeconds(minAfter*60);
        //    options.SetScheduledAt(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Unspecified));
        //    options.SetUserId("2000");
        //    await _jm.Create(options, "Scheduled", "Scheduled note");
        //    return View("Index");

        //}

        //public async Task<ActionResult> CreateRecurring()
        //{
        //    var options = new RecurringTaskOptions();
        //    options.SetTimeZoneId("Central Standard Time");
        //    options.SetCronExpression(Cron.Minutely());
        //    options.SetUserId("4000");
        //    await _jm.Create(options, "Recurring", "Recurring note");
        //    return View("Index");
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Cancel(int id)
        {
            var result = _jm.Cancel(id).Result;
            return View("Index");
        }
    }
}
