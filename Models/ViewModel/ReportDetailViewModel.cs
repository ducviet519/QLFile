using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models
{
    public class ReportDetailViewModel
    {
        public ReportDetail ReportDetail { get; set; }

        public List<ReportDetail> ReportDetails { get; set; }

        public IEnumerable<SelectListItem> Depts { get; set; }
    }
}
