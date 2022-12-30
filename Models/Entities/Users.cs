using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entity
{
    public class Users
    {
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
