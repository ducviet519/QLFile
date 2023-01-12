using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class ThuMuc
    {
    }

    public class ThuMucCha
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parentId { get; set; }
    }
}
