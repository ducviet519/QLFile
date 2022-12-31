using GleamTech.DocumentUltimate.AspNet.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class DocumentViewModel
    {
        public DocumentViewer DocumentViewer { get; set; }
        public string FileLink { get; set; }
        public string FileName { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string IDPhienBan { get; set; }
    }
}
