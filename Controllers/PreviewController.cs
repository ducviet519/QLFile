using GleamTech.DocumentUltimate;
using GleamTech.DocumentUltimate.AspNet;
using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class PreviewController : Controller
    {
        #region Services
        private readonly IUnitOfWork _services;
        private readonly IWebHostEnvironment _hosting;
        public PreviewController(IUnitOfWork services, IWebHostEnvironment hosting)
        {
            _services = services;
            _hosting = hosting;
        }
        #endregion

        #region Kiểm tra quyền người dùng
        private bool PermissionInUser(string permission)
        {
            var data = HttpContext.User.Claims.Where(c => c.Type == "Permission" && c.Value.ToUpper().Contains(permission.ToUpper())).ToList();
            if(data != null  && data.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
        #endregion

        private DocumentViewModel GetDocumentViewer(string documentLink = null, string textmarks = null, string imagemarksPath = null)
        {           
            DocumentViewModel model = new DocumentViewModel();
            model.FileName = System.IO.Path.GetFileName(documentLink);

            if(String.IsNullOrEmpty(documentLink)) { return model = null; }
           
            var documentOptions = new DocumentViewer
            {
                Width = 1100,
                Height = 600,
                Resizable = false,
                Document = documentLink,
                Watermarks = {}
            };

            //Thêm Watermarks                      
            if (!String.IsNullOrEmpty(imagemarksPath))
            {
                ImageWatermark imageWatermark = new ImageWatermark()
                {
                    ImageFile = imagemarksPath,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Opacity = 50,
                    Height = 50,
                    PageRange = "All",
                };
                documentOptions.Watermarks.Add(imageWatermark);
            }
            if (!String.IsNullOrEmpty(textmarks))
            {
                TextWatermark textWatermark = new TextWatermark()
                {
                    Text = textmarks,
                    Rotation = -45,
                    Opacity = 50,
                    FontColor = Color.Red,
                    Width = 50,
                    Height = 50,
                    SizeIsPercentage = true,
                };
                documentOptions.Watermarks.Add(textWatermark);
            }

            //Phân quyền
            if (PermissionInUser("Document.All"))
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
            }
            else if (PermissionInUser("Document.PrintOnly")){
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
                documentOptions.DeniedPermissions = DocumentViewerPermissions.Download | DocumentViewerPermissions.DownloadAsPdf;
            }
            else if (PermissionInUser("Document.DownloadOnly"))
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
                documentOptions.DeniedPermissions = DocumentViewerPermissions.Print;
            }
            else
            {
                documentOptions.AllowedPermissions = DocumentViewerPermissions.All;
                documentOptions.DeniedPermissions = DocumentViewerPermissions.Print | DocumentViewerPermissions.Download | DocumentViewerPermissions.DownloadAsPdf;
            }

            model.DocumentViewer = documentOptions;

            return model;
        }

        public IActionResult GetFile(string file = null)
        {
            string documentLink = Path.Combine(_hosting.WebRootPath, "files\\File Mau.xlsx");
            string imagemarksPath = Path.Combine(_hosting.WebRootPath, "images\\logo_300x300.png");
            string textmarks = "Tâm Anh Hospital";
            DocumentViewModel model = GetDocumentViewer(documentLink, textmarks, imagemarksPath);
           
            return View(model);
        }

        public IActionResult VanBan(string file = null)
        {
            DocumentVM model = new DocumentVM();
            //model.Iframe = @$"<iframe name='myIframe' id='myIframe' src='/Preview/GetFile?file={file}' title='preview' style='width:100%;' frameborder='0' scrolling='no' onload='resizeIframe(this)' allowfullscreen></iframe>";
            model.filePath = file;
            return View(model);
        }
    }
}
