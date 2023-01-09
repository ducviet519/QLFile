using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
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

        public async Task<FileImport> ReadExcelFile(IFormFile fileUpload)
        {
            FileImport data = new FileImport();
            try
            {
                string randomID = System.Guid.NewGuid().ToString("N");
                string[] allowedExtsnions = new string[] { ".xls", ".xlsx", ".csv" };
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "files");
                if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    string dataFileName = Path.GetFileName(fileUpload.FileName);
                    string extension = Path.GetExtension(dataFileName);
                    if (!allowedExtsnions.Contains(extension)) { data.status = "File tải lên không phải file Excel"; data.dataExcels = null; }
                    else
                    {
                        string fileName = $"{randomID}_{fileUpload.FileName}";
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await fileUpload.CopyToAsync(fileStream);
                        }
                        if (File.Exists(filePath))
                        {
                            List<DataExcel> dataExcels = new List<DataExcel>();
                            XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filePath);
                            var sheets = workbook.Worksheets.First();
                            
                            //var sheets = workbook.Worksheet(1).RangeUsed().RowsUsed().Skip(1);
                            var rows = sheets.RangeUsed().RowsUsed().Skip(1).ToList();
                            int stt = 0;
                            foreach (var row in rows)
                            {
                                DataExcel item = new DataExcel()
                                {
                                    stt = (stt + 1).ToString(),
                                    tenvanban = row.Cell(1).Value.ToString(),
                                    mavanban = row.Cell(2).Value.ToString(),
                                    phienban = row.Cell(3).Value.ToString(),
                                    ngayphathanh = row.Cell(4).Value.ToString(),
                                    ngayhieuluc = row.Cell(5).Value.ToString(),
                                    ngayupload = row.Cell(6).Value.ToString() ?? DateTime.Now.ToString("dd/MM/yyyy"),
                                    khoaphongsoanthao = row.Cell(7).Value.ToString(),
                                    nguoisoanthao = row.Cell(8).Value.ToString(),
                                    doituongapdung = row.Cell(9).Value.ToString(),
                                    donviapdung = row.Cell(10).Value.ToString(),
                                    loaivanban = row.Cell(11).Value.ToString(),
                                    tenthumuc = row.Cell(12).Value.ToString(),
                                    mathumuc = row.Cell(13).Value.ToString(),
                                };
                                dataExcels.Add(item);
                            }
                            data.status = "OK";
                            data.filepath = filePath;
                            data.dataExcels = dataExcels;
                            File.Delete(filePath);
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return data;
            }
        }

        public async Task<string> UploadFileAsync(IFormFile fileUpload)
        {
            string FileLink = "";
            string getDateS = DateTime.Now.ToString("ddMMyyyyHHmmss");
            string uploadsFolder = "D:\\VanBan";
            if (!Directory.Exists(uploadsFolder)) { Directory.CreateDirectory(uploadsFolder); }

            if (fileUpload != null && fileUpload.Length > 0)
            {
                string fileName = $"{getDateS}_{fileUpload.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await fileUpload.CopyToAsync(fileStream);
                }
                if (File.Exists(filePath))
                {
                    return FileLink = filePath;
                }
                else { return FileLink = ""; }
            }
            else
                return FileLink = "";
        }

    }
}
