using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Controllers
{
    public class PhanQuyenController : Controller
    {
        public IActionResult QuyenCaNhan()
        {
            return PartialView("_QuyenCaNhan");
        }

        public IActionResult QuyenNhom()
        {
            return PartialView("_QuyenNhom");
        }
    }
}
