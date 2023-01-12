using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebTools.Extensions;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        #region Connnection Database

        private readonly IConfiguration _configuration;
        private readonly IReportListServices _reportListServices;
        private readonly IReportVersionServices _reportVersionServices;
        private readonly IReportSoftServices _reportSoftServices;
        private readonly IReportDetailServices _reportDetailServices;
        private readonly IReportURDServices _reportURDServices;
        private readonly IDepts _depts;
        private readonly ISoftwareServices _softwareServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGoogleDriveAPI _googleDriveAPI;
        private readonly IUploadFileServices _uploadFileServices;

        public ReportController(
            IConfiguration configuration,
            IReportListServices reportListServices,
            IReportVersionServices reportVersionServices,
            IReportSoftServices reportSoftServices,
            IReportDetailServices reportDetailServices,
            IReportURDServices reportURDServices,
            IDepts depts,
            ISoftwareServices softwareServices,
            IWebHostEnvironment webHostEnvironment,
            IUploadFileServices uploadFileServices,
            IGoogleDriveAPI googleDriveAPI
            )
        {
            _configuration = configuration;
            _reportListServices = reportListServices;
            _reportVersionServices = reportVersionServices;
            _reportSoftServices = reportSoftServices;
            _reportDetailServices = reportDetailServices;
            _reportURDServices = reportURDServices;
            _depts = depts;
            _softwareServices = softwareServices;
            _webHostEnvironment = webHostEnvironment;
            _uploadFileServices = uploadFileServices;
            _googleDriveAPI = googleDriveAPI;
        }
        #endregion

        #region Index Page

        public async Task<IActionResult> Index()
        {
            ReportListViewModel model = new ReportListViewModel();
            model.URDs = new SelectList((await _reportURDServices.GetAll_URDAsync()).OrderBy(i => i.Des), "ID", "Des");
            model.ReportLists = await _reportListServices.GetReportListAsync();
            return View(model);
        }

        public async Task<JsonResult> SearchReportList(string searchString, string searchTrangThaiSD, string searchTrangThaiPM, string searchDate, string searchURD, string loai)
        {
            if (!String.IsNullOrEmpty(loai) && loai == "1")
            {
                var data = await _reportListServices.SearchReportListAsync(searchURD, searchString, searchDate, searchTrangThaiSD, searchTrangThaiPM);

                return Json(new { data });
            }
            else if (!String.IsNullOrEmpty(loai) && loai == "2")
            {
                List<GoogleDriveFile> Table = new List<GoogleDriveFile>();
                if (!String.IsNullOrEmpty(searchString)) { Table = await _googleDriveAPI.SearchDriveFiles(searchString); }
                else { Table = null; }
                var data = await _reportListServices.SearchReportNameAsync(searchURD, Table);
                if (!String.IsNullOrEmpty(searchDate))
                {
                    data = data.Where(s => s.NgayBanHanh != null && s.NgayBanHanh.Contains(searchDate.ToString())).ToList();
                }
                if (!String.IsNullOrEmpty(searchTrangThaiSD))
                {
                    switch (searchTrangThaiSD)
                    {
                        case "0":
                            searchTrangThaiSD = "";
                            break;
                        case "1":
                            searchTrangThaiSD = "Đang sử dụng";
                            break;
                        case "2":
                            searchTrangThaiSD = "Chưa ban hành";
                            break;
                        case "3":
                            searchTrangThaiSD = "Đã hủy";
                            break;
                    }
                    data = data.Where(s => s.TrangThai.ToLower().Contains(searchTrangThaiSD.ToLower())).ToList();
                }
                if (!String.IsNullOrEmpty(searchTrangThaiPM))
                {
                    switch (searchTrangThaiPM)
                    {
                        case "0":
                            searchTrangThaiPM = "";
                            break;
                        case "1":
                            searchTrangThaiPM = "Đã hoàn thành";
                            break;
                        case "2":
                            searchTrangThaiPM = "Chưa hoàn thành";
                            break;
                        case "3":
                            searchTrangThaiPM = "Chưa có";
                            break;
                    }
                    data = data.Where(s => s.TrangThaiPM.ToLower().Contains(searchTrangThaiPM.ToLower())).ToList();
                }
                return Json(new { data });
            }
            else
            {
                var data = await _reportListServices.SearchReportListAsync();
                return Json(new { data });
            }
        }
        #endregion

        #region Khử dấu cho string        
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #endregion

        #region Biểu mẫu
        [HttpGet]
        public async Task<IActionResult> AddReport()
        {
            ReportListViewModel model = new ReportListViewModel();
            model.Depts = new SelectList((await _depts.GetAll_DeptsAsync()).OrderBy(i => i.KhoaP), "STT", "KhoaP");
            return PartialView("_AddReportPartial", model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> AddReport_Json(ReportList reportList)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            try
            {
                var data = await _reportListServices.GetReportListAsync();
                data = data.Where(r => r.MaBM != null && StaticHelper.RemoveSpecialCharacters(r.MaBM).ToUpper().Contains(StaticHelper.RemoveSpecialCharacters(reportList.MaBM).ToUpper())).ToList();
                if (data.Count > 0)
                {
                    message = $"Mã biểu mẫu: <b>{reportList.MaBM}</b> đã tồn tại trên hệ thống. Xin vui lòng kiểm tra lại.";
                    title = "Thông báo!";
                    result = "info";
                }
                else
                {
                    reportList.CreatedUser = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    reportList.FileLink = await _uploadFileServices.UploadFileAsync(reportList.fileUpload);
                    if (reportList.fileUpload != null && String.IsNullOrEmpty(reportList.FileLink))
                    {
                        message = $"File <b>{reportList.fileUpload.FileName}</b> tải lên không thành công. Vui lòng kiểm tra lại file.";
                        title = "Lỗi!";
                        result = "error";
                    }
                    else
                    {
                        result = await _reportListServices.InsertReportListAsync(reportList);
                        if (result == "OK")
                        {
                            message = $"Đã thêm biểu mẫu <b>{reportList.TenBM}</b> thành công.";
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

        [HttpGet]
        public async Task<IActionResult> EditReport(string id)
        {
            ReportListViewModel model = new ReportListViewModel();
            model.ReportList = await _reportListServices.GetReportByIDAsync(id);
            model.Depts = new SelectList((await _depts.GetAll_DeptsAsync()).OrderBy(i => i.KhoaP), "STT", "KhoaP");
            return PartialView("_EditReportPartial", model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> EditReport(ReportList reportList)
        {
            reportList.KhoaPhong = Request.Form["KhoaPhong"];
            reportList.CreatedUser = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (reportList.fileUpload != null) { reportList.FileLink = await _uploadFileServices.UploadFileAsync(reportList.fileUpload); }

            var result = await _reportListServices.UpdateReportListAsync(reportList);
            if (result == "OK")
            {
                TempData["SuccessMsg"] = "Cập nhật thông tin Biểu mẫu: " + reportList.TenBM + " thành công!";
            }
            else
            {
                TempData["ErrorMsg"] = "Lỗi! " + result;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> EditReport_Json(ReportList reportList)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            try
            {
                reportList.CreatedUser = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                if (reportList.fileUpload != null) { reportList.FileLink = await _uploadFileServices.UploadFileAsync(reportList.fileUpload); }
                if (reportList.fileUpload != null && String.IsNullOrEmpty(reportList.FileLink))
                {
                    message = $"File <b>{reportList.fileUpload.FileName}</b> tải lên không thành công. Vui lòng kiểm tra lại file.";
                    title = "Lỗi!";
                    result = "error";
                }
                else
                {
                    result = await _reportListServices.UpdateReportListAsync(reportList);
                    if (result == "OK")
                    {
                        message = $"Cập nhật thông tin Biểu mẫu: <b>{reportList.TenBM}</b> thành công.";
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
            }
            catch (Exception ex)
            {
                message = ex.Message;
                title = "Lỗi!";
                result = "error";
            }
            return Json(new { Result = result, Title = title, Message = message });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteReport(string IDBieuMau, string IDPhienBan)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            string filelink = (await _reportListServices.GetReportByIDAsync(IDBieuMau)).FileLink;
            try
            {
                result = await _reportListServices.DeleteReportListAsync(IDBieuMau, IDPhienBan);
                if (result == "OK")
                {
                    if (!String.IsNullOrEmpty(filelink)) { System.IO.File.Delete(filelink); }

                    message = $"Đã xóa biểu mẫu";
                    title = "Thành công!";
                    result = "success";
                }
                else
                {
                    message = $"Lỗi! {result}";
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

        #endregion

        #region Phiên bản
        //4. Tạo chức năng hiển thị phiên bản
        public async Task<IActionResult> Version(string id)
        {
            ReportVersionViewModel model = new ReportVersionViewModel();
            model.VersionList = (await _reportVersionServices.GetReportVersionAsync(id)).LastOrDefault();
            model.VersionLists = (await _reportVersionServices.GetReportVersionAsync(id)).ToList();

            return PartialView("_VesionPartial", model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<JsonResult> AddVersion(ReportVersion reportVersion)
        {
            string message = String.Empty;
            string title = String.Empty;
            string result = String.Empty;
            try
            {
                reportVersion.CreatedUser = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                string getDateS = DateTime.Now.ToString("ddMMyyyy");
                string IDBieuMau = reportVersion.IDBieuMau;
                reportVersion.FileLink = await _uploadFileServices.UploadFileAsync(reportVersion.fileUpload);
                if (reportVersion.fileUpload != null && String.IsNullOrEmpty(reportVersion.FileLink))
                {
                    message = $"File <b>{reportVersion.fileUpload.FileName}</b> tải lên không thành công. Vui lòng kiểm tra lại file.";
                    title = "Lỗi!";
                    result = "error";
                }
                else
                {
                    result = await _reportVersionServices.InsertReportVersionAsync(reportVersion);
                    if (result == "OK")
                    {
                        message = $"Đã xóa biểu mẫu";
                        title = "Thành công!";
                        result = "success";
                    }
                    else
                    {
                        message = $"Lỗi! {result}";
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
            return Json(new { Result = result, Title = title, Message = message });
        }


        [HttpGet]
        public async Task<JsonResult> GetVersionsJson(string id)
        {
            var data = (await _reportVersionServices.GetReportVersionAsync(id)).ToList();
            return Json(new { data });
        }

        public async Task<JsonResult> DeleteVersionsJson(string id, string link)
        {
            var result = await _reportVersionServices.DeleteReportVersionAsync(id);
            if (result == "DEL")
            {
                string resultDelete = _uploadFileServices.DeleteFile(link);
                return Json(new { message = "Xóa phiên bản thành công!" });
            }
            else
            {
                return Json(new { message = "Lỗi! Phiên bản chưa được xóa" });
            }
        }
        #endregion

        #region Phần mềm
        //6. Tạo cửa sổ Phần mềm
        public async Task<IActionResult> Soft(string id)
        {
            ReportSoftViewModel model = new ReportSoftViewModel();
            model.ReportSoft = (await _reportSoftServices.GetReportSoftAsync(id)).FirstOrDefault();
            model.ReportSofts = (await _reportSoftServices.GetReportSoftAsync(id)).ToList();

            //URD SelectList
            model.URDs = new SelectList((await _reportURDServices.GetAll_URDAsync()).OrderBy(i => i.Des), "ID", "Des");

            //Softs SelectList
            model.PhanMems = new SelectList((await _softwareServices.GetSoftwareAsync()).OrderBy(i => i.Name), "ID", "Name");

            return PartialView("_SoftPartial", model);
        }

        //7. Tạo chức năng lưu dữ liệu khi ấn nút Lưu ở phần 6
        [HttpPost]
        public async Task<IActionResult> AddSoft(ReportSoft reportSoft)
        {
            string url = Request.Headers["Referer"].ToString();
            int count = Int32.Parse(Request.Form["count"]);
            for (int i = 0; i < count; i++)
            {
                reportSoft.IDBieuMau = Request.Form["IDBieuMau"];
                reportSoft.IDPhienBan = Request.Form["IDPhienBan-" + i];
                reportSoft.PhanMem = Request.Form["PhanMem"];
                reportSoft.URD = Request.Form["URD"];
                reportSoft.ViTriIn = Request.Form["ViTriIn"];
                reportSoft.CachIn = Request.Form["CachIn"];
                reportSoft.TrangThaiPM = Request.Form["TrangThaiPM-" + i];
                reportSoft.User = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                if (reportSoft.IDBieuMau != null)
                {
                    var result = await _reportSoftServices.InsertReportSoftAsync(reportSoft);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Cập nhật thông tin thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi!" + result;
                    }
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Xem Chi tiết
        //8. Tạo giao diện Chi tiết
        public async Task<IActionResult> Detail(string id)
        {
            ReportDetailViewModel model = new ReportDetailViewModel();
            model.ReportDetail = (await _reportDetailServices.GetReportDetailAsync(id)).FirstOrDefault();
            model.ReportDetails = (await _reportDetailServices.GetReportDetailAsync(id)).ToList();
            model.Depts = new SelectList((await _depts.GetAll_DeptsAsync()).OrderBy(i => i.KhoaP), "STT", "KhoaP");
            return PartialView("_DetailPartial", model);
        }

        //9. Tạo chức năng lưu dữ liệu khi ấn nút Lưu ở phần 8
        public async Task<IActionResult> AddDetail(ReportDetail reportDetail)
        {
            reportDetail.User = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            int count = Int32.Parse(Request.Form["count"]);
            for (int i = 0; i < count; i++)
            {
                reportDetail.IDBieuMau = Request.Form["IDBieuMau"];
                reportDetail.IDPhienBan = Request.Form["IDPhienBan-" + i];
                reportDetail.KhoaPhong = Request.Form["KhoaPhong"];
                reportDetail.GhiChu = Request.Form["GhiChu-" + i];
                reportDetail.TrangThai = Request.Form["TrangThai-" + i];
                reportDetail.User = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.GivenName).Value ?? HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                if (reportDetail.IDBieuMau != null)
                {
                    var result = await _reportDetailServices.InsertReportDetailAsync(reportDetail);
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = "Cập nhật thông tin thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi!" + result;
                    }
                }

            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Document View

        public bool CheckPermissionExist(string permission)
        {
            var check = HttpContext.User.Claims.Where(c => c.Type == "Permission" && c.Value.Contains(permission));
            if (check == null) { return false; }
            else { return true; }
        }

        //Document View
        private async Task<DocumentViewModel> GetDocumentViewerModel(string link)
        {
            var data = (await _reportListServices.GetReportListAsync()).ToList();
            if (!String.IsNullOrEmpty(link))
            {
                data = data.Where(s => !String.IsNullOrEmpty(s.IDPhienBan) && s.IDPhienBan.ToLower().Contains(link.ToLower())).ToList();
            }
            ReportList reportList = data.FirstOrDefault();
            var documentLink = "";
            DocumentViewModel model = new DocumentViewModel();
            model.IDPhienBan = link;
            if (!String.IsNullOrEmpty(reportList.FileLink))
            {
                documentLink = $@"{reportList.FileLink}";
                string name = Path.GetFileName(reportList.FileLink);
                model.FileName = name.Substring(15, name.Length - 15);
                model.Extension = Path.GetExtension(reportList.FileLink);
                model.FileLink = documentLink;
            }
            ClaimsPrincipal currentUser = this.User;
            if (currentUser.IsInRole("Document") || currentUser.IsInRole("Admin"))
            {
                model.DocumentViewer = new DocumentViewer
                {
                    Width = 1100,
                    Height = 600,
                    Resizable = false,
                    Document = documentLink,
                    AllowedPermissions = DocumentViewerPermissions.All,
                    //DisplayMode = GleamTech.AspNet.UI.DisplayMode.Window,
                };
            }
            else
            {
                model.DocumentViewer = new DocumentViewer
                {
                    Width = 1100,
                    Height = 600,
                    Resizable = false,
                    Document = documentLink,
                    AllowedPermissions = DocumentViewerPermissions.All,
                    DeniedPermissions = DocumentViewerPermissions.Print | DocumentViewerPermissions.Download | DocumentViewerPermissions.DownloadAsPdf,
                    //DisplayMode = GleamTech.AspNet.UI.DisplayMode.Window,
                };
            }

            return model;
        }

        public async Task<IActionResult> PopUpDocumentView(string link)
        {
            DocumentViewModel model = await GetDocumentViewerModel(link);
            ViewData["Iframe"] = @$"<iframe name='myIframe' id='myIframe' src='/Report/DocumentView?link={link}' title='preview' style='width:100%;' frameborder='0' scrolling='no' onload='resizeIframe(this)' allowfullscreen></iframe>";
            //return PartialView("_DocumentViewPartial", model);
            return PartialView("_PreviewPartial", model);
        }
        public async Task<IActionResult> DocumentView(string link)
        {
            DocumentViewModel model = await GetDocumentViewerModel(link);
            return PartialView("_DocumentView", model);
        }
        #endregion
    }
}
