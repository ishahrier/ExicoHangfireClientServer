﻿using System;
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
using Newtonsoft.Json;

namespace HFServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IManageJob _jm;
        private readonly IExicoHfDbService _service;

        public IServiceProvider Di { get; }
       // public IManageWork<HfUserRecurringJobModel> Model { get; }

        public HomeController(IManageJob jm, IExicoHfDbService service )
        {
            _jm = jm;
            _service = service;
 
        }
        public IActionResult Index()
        {

            var rec = new HfUserRecurringJobModel()
            {
                Name = "Tst recurring",
                Note = "Test Note",
                UserId = "1111",
                WorkerClass = "The recurring Worker",
                TimeZoneId = "Central Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 10,
                CronExpression = Cron.MinuteInterval(2),

            };

            var data = JsonConvert.SerializeObject(rec);
 
            return View();
        }

        public async Task<IActionResult> CreateFnF()
        {
            var options2 = new HfUserFireAndForgetJobModel()
            {
                Name = "Tst Fnf",
                Note = "Test Note",
                UserId = "1111",
                WorkerClass = "The Fnf Worker",               
                TimeZoneId = "Central Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 10
            };

            var data = await _jm.Create(options2);
            return View("Index");
        }

        public async Task<ActionResult> CreateScheduled(int minAfter = 1)
        {
            var options2 = new HfUserScheduledJobModel()
            {
                Name = "Tst schedule",
                Note = "Test Note",
                UserId = "1111",
                WorkerClass = "The Fnf Worker",
                TimeZoneId = "Eastern Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 10,
                ScheduledAt = DateTimeOffset.Now.AddMinutes(minAfter)
            };

            var data = await _jm.Create(options2);
            return View("Index");

        }

        public async Task<ActionResult> CreateRecurring()
        {
            var options2 = new  HfUserRecurringJobModel()
            {
                Name = "Tst recurring",
                Note = "Test Note",
                UserId = "1111",
                WorkerClass = "The recurring Worker",
                TimeZoneId = "Eastern Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 10,
                CronExpression = Cron.MinuteInterval(2),
                
            };

            var data = await _jm.Create(options2);
            return View("Index");

        }

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
