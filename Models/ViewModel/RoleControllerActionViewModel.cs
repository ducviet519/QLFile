using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Models
{
    public class RoleControllerActionViewModel
    {
        public Roles Roles { get; set; }

        public List<Roles> RolesList { get; set; }

        public ModuleControllers Controllers { get; set; }

        public List<ModuleControllers> ControllerLists { get; set; }

        public ModuleActions Actions { get; set; }
        public List<ModuleActions> ActionLists { get; set; }

        public RoleControllerActions RoleControllerActions { get; set; }
        public List<RoleControllerActions> RoleControllerActionLists { get; set; }

        public IEnumerable<SelectListItem> SelectController { get; set; }
    }
}
