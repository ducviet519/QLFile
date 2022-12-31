using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTools.Models;
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

        [HttpPost]
        public JsonResult GetTreeData(int page = 1, int rows = 10)
        {
            string json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Data\\treegrid2_data.json"));
            Root data = JsonConvert.DeserializeObject<Root>(json);
            return Json( new { rows = data.rows, total = data.total });
        }
    }
}
