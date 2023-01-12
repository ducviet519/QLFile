using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportSoftViewModel
    {
        public ReportSoft ReportSoft { get; set; }

        public List<ReportSoft> ReportSofts { get; set; }

        public List<ReportURD> URDLists { get; set; }

        public IEnumerable<SelectListItem> URDs { get; set; }

        public IEnumerable<SelectListItem> PhanMems { get; set; }
    }
}
