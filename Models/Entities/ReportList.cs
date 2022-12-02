using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public enum SortOder { Ascending=0, Descending=1}
    public class ReportList
    {
        [Key]
        public string IDBieuMau { get; set; }
        public string IDPhienBan { get; set; }

        [StringLength(10)]
        public string PhienBan { get; set; }


        [Required]
        [StringLength(500)]
        public string TenBM { get; set; }

        [Required]
        [StringLength(50)]
        public string MaBM { get; set; }

        [StringLength(1000)]
        public string GhiChu { get; set; }

        [StringLength(100)]
        public string KhoaPhong { get; set; }

        [StringLength(100)]
        public string QuyTrinh { get; set; }

        [StringLength(1000)]
        public string ViTriIn { get; set; }

        [StringLength(1000)]
        public string CachIn { get; set; }

        [StringLength(100)]
        public string URD { get; set; }

        public string NgayBanHanh { get; set; }


        //GET
        public string TheLoai { get; set; }
        public string PhanMem { get; set; }
        public string TrangThai { get; set; }
        public string TrangThaiPM { get; set; }
        //POST
        public int postTheLoai { get; set; }
        public int postPhanMem { get; set; }
        public int postTrangThai { get; set; }
        public int postTrangThaiPM { get; set; }



        public DateTime CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }


        public int? STT { get; set; }

        //1: hiển thị nút 0: ẩn nút trên bảng Phiên bản
        public int PhanMem1 { get; set; }
        public int PhienBan1 { get; set; }

        public string KySo { get; set; }
        public string KyDienTu { get; set; }
        public string KyTay { get; set; }
        public string HSBADienTu { get; set; }


        [StringLength(1000)]
        public string FileLink { get; set; }

        [Required(ErrorMessage = "Chọn một file")]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "png,jpg,jpeg,gif,doc,docx,pdf,txt")]
        [Display(Name = "File:")]
        [BindProperty]
        public IFormFile fileUpload { get; set; }

    }
}
