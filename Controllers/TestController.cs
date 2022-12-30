using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class TestController : Controller
    {
        private readonly IUnitOfWork _services;
        public TestController(IUnitOfWork services) 
        {
            _services = services;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetData()
        {
            var data = await _services.Report_List.GetReportListAsync();
            return Json( new { data = data });
        }
    }
}
