using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportSoft
    {
        public string IDBieuMau { get; set; }
        public string IDPhienBan { get; set; }
        public string TenBM { get; set; }
        public string MaBM { get; set; }
        public string PhanMem { get; set; }
        public string URD { get; set; }
        public string ViTriIn { get; set; }
        public string CachIn { get; set; }
        public int STT { get; set; }
        public string PhienBan { get; set; }
        public DateTime NgayBanHanh { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string FileLink { get; set; }
        public string TrangThaiPM { get; set; }
        public string User { get; set; }

        public int KySo { get; set; }
        public int KyDienTu { get; set; }
        public int KyTay { get; set; }
        public int HSBADienTu { get; set; }

        [EnumDataType(typeof(PhanMem))]
        public PhanMem PhanMemDropList { get; set; }

        //[EnumDataType(typeof(TrangPhaiPM))]
        //public PhanMem TrangThaiPMDropList { get; set; }

        //[EnumDataType(typeof(KySo))]
        //public KySo KySoDropList { get; set; }

        //[EnumDataType(typeof(KyDienTu))]
        //public KyDienTu KyDienTuDropList { get; set; }

        //[EnumDataType(typeof(KyTay))]
        //public KyTay KyTayDropList { get; set; }

        //[EnumDataType(typeof(HSBADienTu))]
        //public HSBADienTu HSBADienTuDropList { get; set; }
    }

    public class PhanMems
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public enum PhanMem
    {
        [Display(Name = "HIS")] HIS = 1,
        [Display(Name = "IVF")] IVF = 2,
        [Display(Name = "HRM")] HRM = 3,
        [Display(Name = "PM Kế toán")] KT = 4
    }

    public enum TrangPhaiPM
    {
        [Display(Name = "Đã hoàn thành")] DaHoanThanh = 1,
        [Display(Name = "Chưa hoàn thành")] ChuaHoanThanh = 2,
        [Display(Name = "Chưa có")] ChuaCo = 3
    }

    public enum KySo
    {
        [Display(Name = "Đã có")] DaCo = 1,
        [Display(Name = "Chưa có")] ChuaCo = 2,
        [Display(Name = "Không dùng")] KhongDung = 3
    }
    public enum KyDienTu
    {
        [Display(Name = "Đã có")] DaCo = 1,
        [Display(Name = "Chưa có")] ChuaCo = 2,
        [Display(Name = "Không dùng")] KhongDung = 3
    }

    public enum KyTay
    {
        [Display(Name = "Có")] Co = 1,
        [Display(Name = "Không")] Khong = 2
    }
    public enum HSBADienTu
    {
        [Display(Name = "Có")] Co = 1,
        [Display(Name = "Không")] Khong = 2
    }
}
