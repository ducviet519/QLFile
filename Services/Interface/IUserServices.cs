using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Models.Entity;

namespace WebTools.Services.Interface
{
    public interface IUserServices
    {
        public List<Users> GetAllUsers();

        public Users GetUsersByID(int? id);

        public Users FindByName(string userName);

        

        public string AddUser(Users users);

        public string Delete(int id);

        public bool IsUserInRole(int UserID, int RoleID);

        public string DeleteRoleInUser(int id);
        public string AddUserRolesByID(UserRoles userRoles);
        public string AddUserRoles(string UserName, string RoleName);
        public UserPermissions RenderPermissions(int ControllerID, int ActionID);

        public string AddUserPermissions(UserPermissions userPermissions);
        public string DeleteUserPermissions(string UserName);

        public List<Roles> GetRoleInUser(string user);
        public Task<List<UserPermissions>> GetAllUserPermissions(string user);
        public Task<List<UserPermissions>> GetDefaultPermissions(string roleName);

    }
}
