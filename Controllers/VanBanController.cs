using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class VanBanController : Controller
    {
        private readonly IUnitOfWork _services;
        public VanBanController(IUnitOfWork services)
        {
            _services = services;
        }

        #region Bảng tin Văn bản - Trang chính

        public IActionResult DanhMuc_VanBan()
        {
            return View();
        }

        public JsonResult DanhMuc_TimKiem()
        {
            return Json(new { });
        }

        public JsonResult DanhMuc_TimKiemNangCao()
        {
            return Json(new { });
        }

        #endregion

        #region Danh sách Văn bản chi tiết
        [HttpGet]
        public IActionResult DanhSach_VanBanChiTiet()
        {
            return View();
        }

        public JsonResult DanhMuc_VanBanChiTiet()
        {
            return Json(new { });
        }

        public JsonResult DoiThuMuc_VanBanChiTiet()
        {
            return Json(new { });
        }
        #endregion

        #region Thư mục
        [HttpGet]
        public IActionResult ThemThuMuc()
        {
            return PartialView("_ThuMuc_ThemMoi");
        }

        [HttpPost]
        public JsonResult Get_ThuMucCha(int page = 1, int rows = 10)
        {
            string json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Data\\combotree_data.json"));
            List<ThuMucCha> data = JsonConvert.DeserializeObject<List<ThuMucCha>>(json);
            return Json(new { rows = data});
        }

        [HttpGet]
        public IActionResult SuaThuMuc()
        {
            return PartialView("_ThuMuc_Sua");
        }
        #endregion

        #region Văn bản
        [HttpGet]
        public IActionResult ImportExcel()
        {
            return PartialView("_VanBan_ThemExcel");
        }

        [HttpPost]
        public async Task<JsonResult> UploadExcelFile(IFormFile fileUpload)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            FileImport data = new FileImport();
            try
            {            
                if(fileUpload != null) 
                {
                    data = await _services.UploadFile.ReadExcelFile(fileUpload);
                    if (data.status == "OK")
                    {
                        
                        message = $"Lấy thành công {data.dataExcels.Count} văn bản";
                        title = "Thành công!";
                        result = "success";
                    }
                    else
                    {
                        message = data.status;
                        title = "Lỗi!";
                        result = "error";
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message, data = data.dataExcels });
        }


        [HttpGet]
        public IActionResult ThemVanBan()
        {
            return PartialView("_VanBan_ThemMoi");
        }

        
        #endregion


    }
}
