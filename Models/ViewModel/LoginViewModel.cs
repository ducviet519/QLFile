using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entity;

namespace WebTools.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
        public string LoginType { get; set; }

        public string UpdatedBy { get; set; }
        public Users Users { get; set; }
        public string ReturnUrl { get; set; }
    }
}
