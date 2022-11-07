using Catel.ExceptionHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebTools.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController>logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 400:
                    ViewBag.ErrorMessage = "Bad Request Error!";
                    ViewBag.Msg = "Máy chủ gặp sự cố khi xử lý truy vấn!";
                    ViewBag.ErrorImage = $"/images/400-error-bad-request.svg";
                    _logger.LogWarning($"Lỗi 400! Path = {statusCodeResult.OriginalPath}" + $" và QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Thông tin đăng nhập không hợp lệ!";
                    ViewBag.Msg = "Xin vui lòng đăng nhập trước khi truy cập!";
                    ViewBag.ErrorImage = $"/images/401-error-unauthorized.svg";
                    _logger.LogWarning($"Lỗi 401! Path = {statusCodeResult.OriginalPath}" + $" và QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
                case 403:
                    ViewBag.ErrorMessage = "Truy cập bị từ chối!";
                    ViewBag.Msg = "Bạn không có quyền truy cập trang web này!";
                    ViewBag.ErrorImage = $"/images/403-error-forbidden.svg";
                    _logger.LogWarning($"Lỗi 403! Path = {statusCodeResult.OriginalPath}" + $" và QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Not Found!";
                    ViewBag.Msg = "Không tìm thấy địa chỉ bạn đang truy cập!";
                    ViewBag.ErrorImage = $"/images/404-error.svg";
                    _logger.LogWarning($"Lỗi 404! Path = {statusCodeResult.OriginalPath}" + $" và QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
                case 503:
                    ViewBag.ErrorMessage = "Không thể kết nối tới Server!";
                    ViewBag.Msg = "Server đang gặp sự cố! Chúng tôi sẽ khắc phục trong thời gian sớm nhất.";
                    ViewBag.ErrorImage = $"/images/503-error-service-unavailable.svg";
                    _logger.LogWarning($"Lỗi 503! Path = {statusCodeResult.OriginalPath}" + $" và QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //ViewBag.ExceptionPath = exceptionDetails.Path;
            //ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            //ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

            _logger.LogError($"Địa chỉ truy cập {exceptionDetails.Path} gặp sự cố" + $"{exceptionDetails.Error}");

            return View("Error");
        }
    }
}
