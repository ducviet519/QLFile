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
                var bangke = await _reportListServices.ExcelData();
                var worksheet = workbook.Worksheets.Add("Biểu mẫu");
                var currentRow = 1;
                var sott = 0;
                worksheet.Cell(currentRow, 1).Value = "STT";
                worksheet.Cell(currentRow, 2).Value = "Tên biểu mẫu";
                worksheet.Cell(currentRow, 3).Value = "Mã biểu mẫu";
                worksheet.Cell(currentRow, 4).Value = "Phiên bản";
                worksheet.Cell(currentRow, 5).Value = "Ngày phát hành";
                worksheet.Cell(currentRow, 6).Value = "Khác";
                worksheet.Cell(currentRow, 7).Value = "Ban Giám Đốc";
                worksheet.Cell(currentRow, 8).Value = "Bộ phận Dinh dưỡng";
                worksheet.Cell(currentRow, 9).Value = "Bộ phận Giặt là";
                worksheet.Cell(currentRow, 10).Value = "Bộ phận HK";
                worksheet.Cell(currentRow, 11).Value = "Bộ phận Truyền thông";
                worksheet.Cell(currentRow, 12).Value = "Phòng Pháp chế và Giám sát nội bộ";
                worksheet.Cell(currentRow, 13).Value = "Khoa Chẩn đoán hình ảnh";
                worksheet.Cell(currentRow, 14).Value = "Khoa Dược";
                worksheet.Cell(currentRow, 15).Value = "Khoa Gây mê hồi sức";
                worksheet.Cell(currentRow, 16).Value = "Khoa Khám bệnh";
                worksheet.Cell(currentRow, 17).Value = "Khoa Ngoại tổng hợp";
                worksheet.Cell(currentRow, 18).Value = "Khoa Ngoại TNNH";
                worksheet.Cell(currentRow, 19).Value = "Khoa Nhi";
                worksheet.Cell(currentRow, 20).Value = "Khoa Nội tổng hợp";
                worksheet.Cell(currentRow, 21).Value = "Trung tâm Sản phụ khoa";
                worksheet.Cell(currentRow, 22).Value = "Khoa Sơ sinh";
                worksheet.Cell(currentRow, 23).Value = "Khoa Tai mũi họng";
                worksheet.Cell(currentRow, 24).Value = "Khoa Xét nghiệm";
                worksheet.Cell(currentRow, 25).Value = "Phòng CNTT";
                worksheet.Cell(currentRow, 26).Value = "Phòng Quản lý Điều dưỡng";
                worksheet.Cell(currentRow, 27).Value = " Phòng HCNS";
                worksheet.Cell(currentRow, 28).Value = "Phòng Kế toán";
                worksheet.Cell(currentRow, 29).Value = "Phòng Kỹ thuật tòa nhà";
                worksheet.Cell(currentRow, 30).Value = "Khoa Tiêu hóa Gan Mật Tụy";
                worksheet.Cell(currentRow, 31).Value = "Phòng QLCL";
                worksheet.Cell(currentRow, 32).Value = "Phòng Trang thiết bị Y tế";
                worksheet.Cell(currentRow, 33).Value = "Phòng Chăm sóc Khách hàng";
                worksheet.Cell(currentRow, 34).Value = "TT Đào tạo và NCKH";
                worksheet.Cell(currentRow, 35).Value = "TT Hỗ trợ sinh sản";
                worksheet.Cell(currentRow, 36).Value = "Khoa Kiểm soát nhiễm khuẩn";
                worksheet.Cell(currentRow, 37).Value = "Phòng Tiêm chủng";
                worksheet.Cell(currentRow, 38).Value = "TT Tế bào gốc";
                worksheet.Cell(currentRow, 39).Value = "Khoa Hồi sức tích cực";
                worksheet.Cell(currentRow, 40).Value = "Khoa Ngoại CTCH";
                worksheet.Cell(currentRow, 41).Value = "Ngân hàng Mô";
                worksheet.Cell(currentRow, 42).Value = "Phòng GSCLDVBV";
                worksheet.Cell(currentRow, 43).Value = "Khoa Giải phẫu bệnh Tế bào";
                worksheet.Cell(currentRow, 44).Value = "Phòng Mua hàng";
                worksheet.Cell(currentRow, 45).Value = "Phòng KHTH Bảo hiểm";
                worksheet.Cell(currentRow, 46).Value = "Khoa Cấp cứu";
                worksheet.Cell(currentRow, 47).Value = "Phòng Tổng đài";
                worksheet.Cell(currentRow, 48).Value = "Khoa Xạ trị";
                worksheet.Cell(currentRow, 49).Value = "Khoa Ung bướu";
                worksheet.Cell(currentRow, 50).Value = "Khoa Nội tiết Đái tháo đường";
                worksheet.Cell(currentRow, 51).Value = "Khoa bệnh nhiệt đới";
                worksheet.Cell(currentRow, 52).Value = "Đội xe và An ninh";
                worksheet.Cell(currentRow, 53).Value = "Khoa Cơ xương khớp";
                worksheet.Cell(currentRow, 54).Value = "Khoa Hô hấp";
                worksheet.Cell(currentRow, 55).Value = "Khoa Phục hồi chức năng";
                worksheet.Cell(currentRow, 56).Value = "Khoa Thần kinh Đột quỵ";
                worksheet.Cell(currentRow, 57).Value = "Bộ phận học việc";
                worksheet.Cell(currentRow, 58).Value = "Khoa Tim mạch";
                worksheet.Cell(currentRow, 59).Value = "Tư vấn Gói sản phẩm";
                worksheet.Cell(currentRow, 60).Value = "Đội sàng lọc";
                worksheet.Cell(currentRow, 61).Value = "Khoa Chống đau và chăm sóc giảm nhẹ";
                worksheet.Cell(currentRow, 62).Value = "Khoa Răng Hàm Mặt";
                worksheet.Cell(currentRow, 63).Value = "Khoa Mắt";
                worksheet.Cell(currentRow, 64).Value = "Trung tâm GMHS và chống đau";
                worksheet.Cell(currentRow, 65).Value = "Trung tâm chẩn đoán hình ảnh";
                worksheet.Cell(currentRow, 66).Value = "Khoa Thẩm mĩ";
                worksheet.Cell(currentRow, 67).Value = "Khoa Dinh dưỡng";
                worksheet.Cell(currentRow, 68).Value = "Phòng Công tác xã hội";
                worksheet.Cell(currentRow, 69).Value = "Ban Tổng Giám đốc";

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
                        worksheet.Cell(currentRow, 6).Value = (item.Khac ?? "").ToString();
                        worksheet.Cell(currentRow, 7).Value = (item.BanGiamdoc ?? "").ToString();
                        worksheet.Cell(currentRow, 8).Value = (item.BophanDanhduong ?? "").ToString();
                        worksheet.Cell(currentRow, 9).Value = (item.BophanGiatla ?? "").ToString();
                        worksheet.Cell(currentRow, 10).Value = (item.BoPhanHK ?? "").ToString();
                        worksheet.Cell(currentRow, 11).Value = (item.Bophantruyenthong ?? "").ToString();
                        worksheet.Cell(currentRow, 12).Value = (item.PhongPhapchevaGiamsatnoibo ?? "").ToString();
                        worksheet.Cell(currentRow, 13).Value = (item.KhoaChandoanhinhanh ?? "").ToString();
                        worksheet.Cell(currentRow, 14).Value = (item.KhoaDuoc ?? "").ToString();
                        worksheet.Cell(currentRow, 15).Value = (item.KhoaGaymehoisuc ?? "").ToString();
                        worksheet.Cell(currentRow, 16).Value = (item.KhoaKhambenh ?? "").ToString();
                        worksheet.Cell(currentRow, 17).Value = (item.KhoaNgoaiTonghop ?? "").ToString();
                        worksheet.Cell(currentRow, 18).Value = (item.KhoaNgoaiTNNH ?? "").ToString();
                        worksheet.Cell(currentRow, 19).Value = (item.KhoaNhi ?? "").ToString();
                        worksheet.Cell(currentRow, 20).Value = (item.KhoaNoiTonghop ?? "").ToString();
                        worksheet.Cell(currentRow, 21).Value = (item.TrungtamSanphukhoa ?? "").ToString();
                        worksheet.Cell(currentRow, 22).Value = (item.KhoaSosinh ?? "").ToString();
                        worksheet.Cell(currentRow, 23).Value = (item.KhoaTaimuihong ?? "").ToString();
                        worksheet.Cell(currentRow, 24).Value = (item.KhoaXetnghiem ?? "").ToString();
                        worksheet.Cell(currentRow, 25).Value = (item.PhongCNTT ?? "").ToString();
                        worksheet.Cell(currentRow, 26).Value = (item.PhongQuanlyDieuduong ?? "").ToString();
                        worksheet.Cell(currentRow, 27).Value = (item.PhongHCNS ?? "").ToString();
                        worksheet.Cell(currentRow, 28).Value = (item.PhongKetoan ?? "").ToString();
                        worksheet.Cell(currentRow, 29).Value = (item.PhongKythuattoanha ?? "").ToString();
                        worksheet.Cell(currentRow, 30).Value = (item.KhoaTieuhoaGanMatTuy ?? "").ToString();
                        worksheet.Cell(currentRow, 31).Value = (item.PhongQLCL ?? "").ToString();
                        worksheet.Cell(currentRow, 32).Value = (item.PhongTrangthietbiYte ?? "").ToString();
                        worksheet.Cell(currentRow, 33).Value = (item.PhongChamsocKhachhang ?? "").ToString();
                        worksheet.Cell(currentRow, 34).Value = (item.TTDaotaovaNCKH ?? "").ToString();
                        worksheet.Cell(currentRow, 35).Value = (item.TTHotrosinhsan ?? "").ToString();
                        worksheet.Cell(currentRow, 36).Value = (item.KhoaKiemsoatnhiemkhuan ?? "").ToString();
                        worksheet.Cell(currentRow, 37).Value = (item.PhongTiemchung ?? "").ToString();
                        worksheet.Cell(currentRow, 38).Value = (item.TTTebaogoc ?? "").ToString();
                        worksheet.Cell(currentRow, 39).Value = (item.KhoaHoisuctichcuc ?? "").ToString();
                        worksheet.Cell(currentRow, 40).Value = (item.KhoaNgoaiCTCH ?? "").ToString();
                        worksheet.Cell(currentRow, 41).Value = (item.NganhangMo ?? "").ToString();
                        worksheet.Cell(currentRow, 42).Value = (item.PhongGSCLDVBV ?? "").ToString();
                        worksheet.Cell(currentRow, 43).Value = (item.KhoaGiaiphaubenhTebao ?? "").ToString();
                        worksheet.Cell(currentRow, 44).Value = (item.PhongMuahang ?? "").ToString();
                        worksheet.Cell(currentRow, 45).Value = (item.PhongKHTHBaohiem ?? "").ToString();
                        worksheet.Cell(currentRow, 46).Value = (item.KhoaCapcuu ?? "").ToString();
                        worksheet.Cell(currentRow, 47).Value = (item.PhongTongdai ?? "").ToString();
                        worksheet.Cell(currentRow, 48).Value = (item.KhoaXatri ?? "").ToString();
                        worksheet.Cell(currentRow, 49).Value = (item.KhoaUngbuou ?? "").ToString();
                        worksheet.Cell(currentRow, 50).Value = (item.KhoaNoitietDaithaoduong ?? "").ToString();
                        worksheet.Cell(currentRow, 51).Value = (item.KhoaBenhnhietdoi ?? "").ToString();
                        worksheet.Cell(currentRow, 52).Value = (item.DoixevaAnninh ?? "").ToString();
                        worksheet.Cell(currentRow, 53).Value = (item.KhoaCoxuongkhop ?? "").ToString();
                        worksheet.Cell(currentRow, 54).Value = (item.KhoaHohap ?? "").ToString();
                        worksheet.Cell(currentRow, 55).Value = (item.KhoaPhuchoichucnang ?? "").ToString();
                        worksheet.Cell(currentRow, 56).Value = (item.KhoaThankinhDotquy ?? "").ToString();
                        worksheet.Cell(currentRow, 57).Value = (item.BophanHocviec ?? "").ToString();
                        worksheet.Cell(currentRow, 58).Value = (item.KhoaTimmach ?? "").ToString();
                        worksheet.Cell(currentRow, 59).Value = (item.TuvanGoisanpham ?? "").ToString();
                        worksheet.Cell(currentRow, 60).Value = (item.Doisangloc ?? "").ToString();
                        worksheet.Cell(currentRow, 61).Value = (item.KhoaChongdauvachamsocgiamnhe ?? "").ToString();
                        worksheet.Cell(currentRow, 62).Value = (item.KhoaRangHamMat ?? "").ToString();
                        worksheet.Cell(currentRow, 63).Value = (item.KhoaMat ?? "").ToString();
                        worksheet.Cell(currentRow, 64).Value = (item.TrungtamGMHSvachongdau ?? "").ToString();
                        worksheet.Cell(currentRow, 65).Value = (item.Trungtamchandoanhinhanh ?? "").ToString();
                        worksheet.Cell(currentRow, 66).Value = (item.KhoaThammi ?? "").ToString();
                        worksheet.Cell(currentRow, 67).Value = (item.KhoaDinhduong ?? "").ToString();
                        worksheet.Cell(currentRow, 68).Value = (item.PhongCongtacxahoi ?? "").ToString();
                        worksheet.Cell(currentRow, 69).Value = (item.BanTongGiamdoc ?? "").ToString();
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
