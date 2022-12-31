using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebTools.Context;

namespace WebTools.Models
{
    public class ReportListViewModel
    {
        public ReportList ReportList { get; set; }

        public List<ReportList> ReportLists { get; set; }

        public PagingList<ReportList> PagingLists { get; set; }

        public IEnumerable<SelectListItem> Depts { get; set; }

        public IEnumerable<SelectListItem> URDs { get; set; }

    }
}
