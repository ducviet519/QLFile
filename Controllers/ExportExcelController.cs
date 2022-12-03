using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services;

namespace WebTools.Controllers
{
    public class ExportExcelController : Controller
    {
        private readonly IReportListServices _reportListServices;
        public ExportExcelController(IReportListServices reportListServices)
        {
            _reportListServices = reportListServices;
        }
        public async Task<IActionResult> DanhSachBieuMauExcel()
        {
            string hoten = String.Empty;
            using (var workbook = new XLWorkbook())
            {
                var bangke = await _reportListServices.GetReportListAsync();
                var worksheet = workbook.Worksheets.Add("Danh sach Bieu mau");
                var currentRow = 1;
                var sott = 0;
                worksheet.Cell(currentRow, 1).Value = "STT";
                worksheet.Cell(currentRow, 2).Value = "Tên biểu mẫu";
                worksheet.Cell(currentRow, 3).Value = "Mã biểu mẫu";
                worksheet.Cell(currentRow, 4).Value = "Phiên bản";
                worksheet.Cell(currentRow, 5).Value = "Ngày phát hành";
                worksheet.Cell(currentRow, 6).Value = "Trạng thái";
                worksheet.Cell(currentRow, 7).Value = "Phần mềm";
                worksheet.Cell(currentRow, 8).Value = "Trạng thái phần mềm";
                worksheet.Cell(currentRow, 9).Value = "Ký số";
                worksheet.Cell(currentRow, 10).Value = "Ký điện tử";
                worksheet.Cell(currentRow, 11).Value = "Ký tay";
                worksheet.Cell(currentRow, 12).Value = "HSBA Điện tử";
                worksheet.Cell(currentRow, 13).Value = "Link File";
                worksheet.Row(currentRow).Style.Font.SetBold();
                foreach (var item in bangke)
                {
                    currentRow++;
                        sott++;
                        worksheet.Cell(currentRow, 1).Value = item.STT;
                        worksheet.Cell(currentRow, 2).Value = (item.TenBM ?? "").ToString();
                        worksheet.Cell(currentRow, 3).Value = (item.MaBM ?? "").ToString();
                        worksheet.Cell(currentRow, 4).Value = (item.PhienBan ?? "").ToString();
                        worksheet.Cell(currentRow, 5).Value = (item.NgayBanHanh ?? "").ToString();
                        worksheet.Cell(currentRow, 6).Value = (item.TrangThai ?? "").ToString();
                        worksheet.Cell(currentRow, 7).Value = (item.PhanMem ?? "").ToString();
                        worksheet.Cell(currentRow, 8).Value = (item.TrangThaiPM ?? "").ToString();
                        worksheet.Cell(currentRow, 9).Value = (item.KySo ?? "").ToString();
                        worksheet.Cell(currentRow, 10).Value = (item.KyDienTu ?? "").ToString();
                        worksheet.Cell(currentRow, 11).Value = (item.KyTay ?? "").ToString();
                        worksheet.Cell(currentRow, 12).Value = (item.HSBADienTu ?? "").ToString();
                        worksheet.Cell(currentRow, 13).Value = (item.FileLink ?? "").ToString();
                }
                worksheet.Columns().AdjustToContents();
                worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Danh sách Biểu mẫu.xlsx");
                }
            }
        }
    }
}
