using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    public class GopYController : Controller
    {
        private readonly IGopYServices _gopYServices;
        public GopYController(IGopYServices gopYServices)
        {
            _gopYServices = gopYServices;
        }
        public IActionResult Index(string IDBieuMau)
        {
            Users model = new Users() 
            {
                UserName = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value,
                Email = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                DisplayName = IDBieuMau ?? "",
            };
            return PartialView("_GopY",model);
        }

        [HttpPost]
        public async Task<JsonResult> Them_GopY(string IDBieuMau, string noidung)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            try
            {
                string user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                result = await _gopYServices.ThemGopY(IDBieuMau,noidung,user);
                if (result == "OK")
                {
                    message = $"Tiếp nhận góp ý thành công";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = result;
                    title = "Lỗi!";
                    result = "error";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }
    }
}
