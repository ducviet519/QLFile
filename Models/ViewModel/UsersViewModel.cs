using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Models.Entity;

namespace WebTools.Models
{
    public class UsersViewModel
    {
        public int UserID { get; set; }
        public int RolerID { get; set; }
        public Users Users { get; set; }

        public List<Users> UsersList { get; set; }

        public List<Roles> RolesList { get; set; }

        public Roles Roles { get; set; }

        public UserPermissions UserPermissions { get; set; }

        public List<UserPermissions> UserPermissionLists { get; set; }
    }
}
