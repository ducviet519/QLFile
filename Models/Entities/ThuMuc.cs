using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class ThuMuc
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ThuMucCha
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public List<ThuMucCha> children { get; set; } = new List<ThuMucCha>();
    }

    public class TreeData
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string title { get; set; }
        public bool expanded { get; set; }
        public bool folder { get; set; }
        public string type { get; set; }
        public string author { get; set; }
        public int year { get; set; }
        public int qty { get; set; }
        public decimal price { get; set; }
        public List<TreeData> children { get; set; } = new List<TreeData>();
    }
}
