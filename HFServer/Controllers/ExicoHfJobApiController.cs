//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Exico.HF.Common.DomainModels;
//using Exico.HF.DbAccess.Db.Services;
//using Exico.HF.DbAccess.Managers;
//using HFServer.Models;
//using Microsoft.AspNetCore.Hosting.Server;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace HFServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ExicoHfJobApiController : ControllerBase
//    {
//        private readonly IManageJob _jm;
//        private readonly ILogger<HomeController> _logger;
//        private readonly IExicoHfDbService _service;
//        private readonly IServiceProvider _provider;

//        public IServiceProvider Di { get; }

//        public ExicoHfJobApiController(IManageJob jm, ILogger<HomeController> logger, IExicoHfDbService service, IServiceProvider provider)
//        {
//            _jm = jm;
//            _logger = logger;
//            _service = service;
//            _provider = provider;
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateFnFJob([FromBody] HfUserFireAndForgetJobModel jobData)
//        {
//            var data = await _jm.Create(jobData);
//            return Ok(data);
//        }


//        [HttpPost]
//        public async Task<IActionResult> CreateScheduledJob([FromBody] HfUserScheduledJobModel jobData)
//        {
//            var data = await _jm.Create(jobData);
//            return Ok(data);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateRecurringJob([FromBody] HfUserRecurringJobModel jobData)
//        {
//            var data = await _jm.Create(jobData);
//            return Ok(data); ;
//        }
 
//        [HttpPost]
//        public Task<IActionResult> Cancel(int id)
//        {
//            var result = _jm.Cancel(id);
//            return View("Index");
//        }

//        public ActionResult Run(int id)
//        {
//            var result = _jm.RunNow(id);
//            return View("Index");
//        }
//    }
//}
