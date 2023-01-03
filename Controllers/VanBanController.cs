using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public IActionResult ThemVanBan()
        {
            return PartialView("_VanBan_ThemMoi");
        }

        
        #endregion


    }
}
