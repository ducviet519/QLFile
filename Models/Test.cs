using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class Test
    {
        
    }
    public class Footer
    {
        public string region { get; set; }
        public int f1 { get; set; }
        public int f2 { get; set; }
        public int f3 { get; set; }
        public int f4 { get; set; }
        public int f5 { get; set; }
        public int f6 { get; set; }
        public int f7 { get; set; }
        public int f8 { get; set; }
    }

    public class Root
    {
        public int total { get; set; }
        public List<Row> rows { get; set; }
        public List<Footer> footer { get; set; }
    }

    public class Row
    {
        public int id { get; set; }
        public string region { get; set; }
        public string state { get; set; }
        public int? status { get; set; }
        public int? action { get; set; }
        public int? f1 { get; set; }
        public int? f2 { get; set; }
        public int? f3 { get; set; }
        public int? f4 { get; set; }
        public int? f5 { get; set; }
        public int? f6 { get; set; }
        public int? f7 { get; set; }
        public int? f8 { get; set; }
        public int? _parentId { get; set; }
    }
}
