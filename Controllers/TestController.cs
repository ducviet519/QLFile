using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTools.Services;

namespace WebTools.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<JsonResult> GetData2(int page = 1, int rows = 10)
        {
            var data = await _services.Report_List.GetReportListAsync();
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            data = data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            var jsonData = new
            {
                total = totalPages,
                rows = data
            };
            return Json(new { data = jsonData });
        }

        [HttpPost]
        public async Task<JsonResult> GetData(int page = 1, int rows = 10)
        {
            var data = await _services.Report_List.GetReportListAsync();
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            data = data.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Json( new { rows = data, total = totalPages });
        }
    }
}
