using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Services;
using Exico.HF.DbAccess.Managers;
using Hangfire;
using HFServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Exico.HF.DbAccess.Managers.Interfaces;

namespace HFServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IManageJob _jm;
        private readonly ILogger<HomeController> _logger;
        private readonly IExicoHfDbService _service;
        private readonly IServiceProvider _provider;

        public IServiceProvider Di { get; }
        // public IManageWork<HfUserRecurringJobModel> Model { get; }

        public HomeController(IManageJob jm, ILogger<HomeController> logger, IExicoHfDbService service, IServiceProvider provider)
        {
            _jm = jm;
            this._logger = logger;
            _service = service;
            this._provider = provider;
        }
        public IActionResult Index()
        {

            //var rec = new HfUserRecurringJobModel()
            //{
            //    Name = "Tst recurring",
            //    Note = "Test Note",
            //    UserId = "1111",
            //    WorkerClass = "The recurring Worker",
            //    TimeZoneId = "Central Standard Time",
            //    Status = Exico.HF.Common.Enums.JobStatus.None,
            //    WorkDataId = 10,
            //    CronExpression = Cron.MinuteInterval(2),

            //};

            //var data = JsonConvert.SerializeObject(rec);

            //var t = Type.GetType("Exico.HF.Common.Interfaces.DownloadAllProducts, Exico.HF.Common");
            //var obj = (IWorker)ActivatorUtilities.CreateInstance(this.proider, t);

            return View();
        }

        public async Task<IActionResult> CreateFnF2()
        {
            var options2 = new HfUserFireAndForgetJobModel()
            {
                Name = "Tst Fnf",
                Note = "Test Note",
                UserId = "1111",
                WorkerClassName = "Exico.HF.Common.Interfaces.DownloadAllProducts",
                WorkerAssemblyName = "Exico.HF.Common",
                TimeZoneId = "Central Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 10
            };

            var data = await _jm.Create(options2);
            return View("Index");
        }
 
        public async Task<IActionResult> CreateFnF()
        {
            var options2 = new HfUserFireAndForgetJobModel()
            {
                Name = "Tst Fnf",
                Note = "Test Note",
                UserId = "1111",
                WorkerClassName = "ExampleThirdPartyWorker.IDownloadFromGql",
                WorkerAssemblyName = "ExampleThirdPartyWorker",
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
                WorkerClassName = "Exico.HF.Common.Interfaces.DownloadAllProducts",
                WorkerAssemblyName = "Exico.HF.Common",
                TimeZoneId = "Eastern Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 11,
                ScheduledAt = DateTimeOffset.Now.AddMinutes(minAfter)
            };

            var data = await _jm.Create(options2);
            return View("Index");

        }

        public async Task<ActionResult> CreateRecurring()
        {
            var options2 = new HfUserRecurringJobModel()
            {
                Name = "Tst recurring",
                Note = "Test Note",
                UserId = "1111",
                WorkerClassName = "Exico.HF.Common.Interfaces.DownloadAllProducts",
                WorkerAssemblyName = "Exico.HF.Common",
                TimeZoneId = "Eastern Standard Time",
                Status = Exico.HF.Common.Enums.JobStatus.None,
                WorkDataId = 12,
                CronExpression = Cron.MinuteInterval(1)

            };

            var data = await _jm.Create(options2);
            return View("Index");

        }

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

        public ActionResult Run(int id)
        {
            var result = _jm.RunNow(id);
            return View("Index");
        }

    }
}
