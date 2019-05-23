﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Exico.HF.Common.TasksOptionsImpl;
using Exico.HF.DbAccess.Managers;
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

        public IActionResult Privacy()
        {
            //var options = new FireAndForgetTaskOptions();
            //options.SetTimeZoneId("Central Standard Time");
            //options.SetUserId("1000");
            //_jm.Create(options, "The name", "the note");

            //var options = new ScheduledTaskOptions();
            //options.SetTimeZoneId("Central Standard Time");
            //options.SetScheduledAt(new DateTime(2019, 5, 23, 9, 17, 00, DateTimeKind.Unspecified));
            //options.SetUserId("2000");
            //_jm.Create(options, "The name", "the note");

            var options = new RecurringTaskOptions();
            options.SetTimeZoneId("Central Standard Time");
            options.SetCronExpression("* * * * * *");
            options.SetUserId("3000");
            _jm.Create(options, "The name", "the note");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
