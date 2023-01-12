using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class FileImport
    {
        public string status { get; set; }
        public string filepath { get; set; }
        public List<DataExcel> dataExcels { get; set; }
    }

    public class DataExcel
    {
        public string stt { get; set; }
        public string tenvanban { get; set; }
        public string mavanban { get; set; }
        public string phienban { get; set; }
        public string ngayphathanh { get; set; }
        public string ngayhieuluc { get; set; }
        public string ngayupload { get; set; }
        public string khoaphongsoanthao { get; set; }
        public string nguoisoanthao { get; set; }
        public string doituongapdung { get; set; }
        public string donviapdung { get; set; }
        public string loaivanban { get; set; }
        public string tenthumuc { get; set; }
        public string mathumuc { get; set; }
    }
}
