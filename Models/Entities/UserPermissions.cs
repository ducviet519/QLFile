using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class UserPermissions
    {
        public string Permission { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public int ControllerID { get; set; }
        public int ActionID { get; set; }
    }
}
