﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.Common.TasksOptionsImpl;
using Exico.HF.DbAccess.Managers;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using HFServer.Models;

namespace HFServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobManager _jm;

        public HomeController(IJobManager jm)
        {
            _jm = jm;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateFnF()
        {
            var options2= new FireAndForgetTaskOptions();
            options2.SetTimeZoneId("Central Standard Time");
            options2.SetUserId("1000");
            await _jm.Create(options2, "Fnf", "Fnf note");
            return Ok("Fire and forget task created");
        }
        >
        public async Task<ActionResult> CreateScheduled(int minAfter=1)
        {

            var options = new ScheduledTaskOptions();
            options.SetTimeZoneId("Central Standard Time");
            var now = DateTime.Now.AddSeconds(minAfter*60);
            options.SetScheduledAt(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Unspecified));
            options.SetUserId("2000");
            await _jm.Create(options, "Scheduled", "Scheduled note");
            return Ok("Scheduled task created");

        }

        public async Task<ActionResult> CreateRecurring()
        {
            var options = new RecurringTaskOptions();
            options.SetTimeZoneId("Central Standard Time");
            options.SetCronExpression(Cron.Minutely());
            options.SetUserId("4000");
            await _jm.Create(options, "Recurring", "Recurring note");
            return Ok("Recurring task created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Cancel(int id)
        {
            var result = _jm.Cancel(id).Result;
            return  Ok();
        }
    }
}
