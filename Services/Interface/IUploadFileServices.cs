using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IUploadFileServices
    {
        Task<string> UploadFileAsync(IFormFile fileUpload);

        string DeleteFile(string filePath);

        Task<FileImport> ReadExcelFile(IFormFile fileUpload);
    }
}
