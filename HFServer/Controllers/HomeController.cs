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
            var options = new FireAndForgetTaskOptions()
            {
                UserId = "1234567",
                TimeZoneId = "abra ka debra"
            };
            
            _jm.Create(options,"The name","the note");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
