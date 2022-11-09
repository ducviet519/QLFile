using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class UploadFileServices : IUploadFileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UploadFileServices(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string DeleteFile(string filePath)
        {
            string result = "";
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                result = "Đã xóa file thành công";
            }
            else
            {
                result = "Lỗi! Xóa file không thành công";
            }
            return result;
        }

        public async Task<string> UploadFileAsync(IFormFile fileUpload)
        {
            string FileLink = "";
            string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
            //string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Upload");
            string uploadsFolder = "D:\\VanBan\\My Drive\\VBNB FileUpload";
            if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }

            if (fileUpload != null && fileUpload.Length > 0)
            {
                string fileName = $"{getDateS}_{fileUpload.FileName}";
                //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.FileName);

                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await fileUpload.CopyToAsync(fileStream);
                }

                //string fileGoogleID = _googleDriveAPI.UploadFile(filePath);
                return FileLink = filePath;
            }
            else
                return FileLink = "";
        }

        public Task<string> UploadMultipleFileAsync(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
