using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Authorization;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Authorize(Policy = "AdminOnly")]
    /// <summary>
    /// Permission Users, Roles, ModuleControllers, ModuleActions
    /// </summary>
    #region Connection Database

    public class PermissionController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRolesServices _roleServices;
        private readonly IModuleControllerServices _moduleControllerServices;
        private readonly IModuleActionServices _moduleActionServices;

        public PermissionController(IUserServices userServices, IRolesServices roleServices, IModuleControllerServices moduleControllerServices, IModuleActionServices moduleActionServices)
        {
            _userServices = userServices;
            _roleServices = roleServices;
            _moduleControllerServices = moduleControllerServices;
            _moduleActionServices = moduleActionServices;
        }

        #endregion

        #region Permission
        //Tạo chuỗi Permission từ Controller và Actions
        public List<string> GeneratePermissionsForModule(int controllerID)
        {
            var permissionList = new List<string>();
            var controllerData = _moduleControllerServices.GetModuleControllersByID(controllerID);
            foreach(var action in _moduleActionServices.GetActionsInController(controllerID))
            {
                string permissionString = $"Permissions.{controllerData.ControllerName}.{action.ActionName}";
                permissionList.Add(permissionString);
            }
            return permissionList;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        #region User
        //[ControllerModelConvention]
        public IActionResult Users()
        {
            UsersViewModel model = new UsersViewModel();
            model.UsersList = _userServices.GetAllUsers();
            //model.Roles = new SelectList(_roleServices.GetAllRoles(), "RoleID", "RoleName");
            model.RolesList = _roleServices.GetAllRoles();
            return View(model);
        }


        [HttpGet]
        //[ControllerModelConvention]
        public IActionResult UserRoles(int? id)
        {
            var viewModel = new List<Roles>();
            var user = _userServices.GetUsersByID(id);
            foreach (var role in _roleServices.GetAllRoles())
            {
                var roles = new Roles
                {
                    RoleID = role.RoleID,
                    RoleName = role.RoleName,
                    Description = role.Description,
                };
                if (_userServices.IsUserInRole(user.UserID, role.RoleID))
                {
                    roles.Status = true;
                }
                else
                {
                    roles.Status = false;
                }
                viewModel.Add(roles);
            }

            UsersViewModel model = new UsersViewModel();
            model.RolesList = viewModel;
            model.Users = _userServices.GetUsersByID(id);

            return PartialView("_UserRolesPartial", model);
        }

        [HttpPost]
        public IActionResult EditUserRole(UserRoles userRoles)
        {
            int count = Int32.Parse(Request.Form["count"]);
            if (count > 0) 
            {
                _userServices.DeleteRoleInUser(userRoles.UserID);
            }
            for (int i = 0; i < count; i++)
            {
                userRoles.UserID = Int32.Parse(Request.Form["UserID-" + i]);
                userRoles.RoleID = Int32.Parse(Request.Form["RoleID-" + i]);
                userRoles.RoleName = Request.Form["RoleName-" + i];
                userRoles.Description = Request.Form["Description-" + i];
                userRoles.Status = Boolean.TryParse(Request.Form["Status-" + i], out bool b);
                if (userRoles.UserID > 0 && userRoles.RoleID > 0 && userRoles.Status == true)
                {
                    var result = _userServices.AddUserRolesByID(userRoles);
                    
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = $"Người dùng đã được cập nhật lại Role thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi! " + result;
                    }
                }
            }

            return RedirectToAction("Users");

        }
        #endregion

        #region Roles
        /// <summary>
        /// Quản lý vai trò
        /// </summary>
        public IActionResult Roles()
        {
            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.RolesList = _roleServices.GetAllRoles();
            model.Roles = null;
            return View(model);
        }

        [HttpGet]
        public IActionResult EditRoles(int? id)
        {
            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.Roles = _roleServices.GetRolesByID(id);
            return PartialView("_EditRolePartial", model);
        }
        [HttpPost]
        public IActionResult EditRoles(Roles roles)
        {
            roles.ModifiedBy = User.Identity.Name;
            roles.DateModified = DateTime.Today;

            if (roles.RoleID > 0 && roles.RoleName != null)
            {
                var result = _roleServices.EditRoles(roles);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Role: {roles.RoleName} đã được cập nhật lại thông tin thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Roles");
            }
            TempData["ErrorMsg"] = $"Lỗi! Không tìm thấy thông tin Roles: {roles.RoleName}";
            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult CreateRoles(Roles roles)
        {
            roles.CreatedBy = User.Identity.Name;
            roles.DateCreated = DateTime.Today;

            if (roles.RoleName != null)
            {
                var result = _roleServices.AddRoles(roles);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Thêm Role: {roles.RoleName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Roles");
            }
            TempData["ErrorMsg"] = "Lỗi! Không thể thêm Roles";
            return RedirectToAction("Roles");
        }

        public IActionResult DeleteRoles(int? id)
        {
            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.Roles = _roleServices.GetRolesByID(id);
            if (id != null)
            {
                var result = _roleServices.DeleteRoles(id);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = "Xóa Role: " + model.Roles.RoleName + " thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("Roles");
            }
            TempData["ErrorMsg"] = "Lỗi! Không thể xóa Role mặc định";
            return RedirectToAction("Roles");
        }

        public IActionResult RoleActions(int id)
        {
            var viewModel = new List<RoleControllerActions>();
            var role = _roleServices.GetRolesByID(id);
            foreach (var action in _moduleActionServices.GetAllModuleActions())
            {
                ModuleControllers controllerData = new ModuleControllers();
                controllerData = _moduleControllerServices.GetModuleControllersByID(action.ControllerID);
                var data = new RoleControllerActions
                {
                    Controller_ID = controllerData.ControllerID,
                    ControllerName = controllerData.ControllerName,
                    ActionID = action.ActionID,
                    ActionName = action.ActionName,
                    Description = action.Description,
                };
                if (_roleServices.IsRoleInControllerAction(role.RoleID, controllerData.ControllerID, data.ActionID))
                {
                    data.Status = true;
                }
                else
                {
                    data.Status = false;
                }
                viewModel.Add(data);
            }

            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.RoleControllerActionLists = viewModel;
            model.Roles = _roleServices.GetRolesByID(id);

            return PartialView("_RoleControllerActionPartial", model);
        }

        public IActionResult AddRoldAction(RoleControllerActions roleControllerActions)
        {
            int count = Int32.Parse(Request.Form["count"]);
            if(count > 0) { _roleServices.DeleteRoleControllerAction(roleControllerActions.RoleID); }
            for (int i = 0; i < count; i++)
            {
                roleControllerActions.RoleID = Int32.Parse(Request.Form["RoleID-" + i]);
                roleControllerActions.Controller_ID = Int32.Parse(Request.Form["ControllerID-" + i]);
                roleControllerActions.ActionID = Int32.Parse(Request.Form["ActionID-" + i]);
                roleControllerActions.Description = Request.Form["Description-" + i];
                roleControllerActions.Status = Boolean.TryParse(Request.Form["Status-" + i], out bool b);
                if (roleControllerActions.ActionID > 0 && roleControllerActions.Controller_ID > 0 && roleControllerActions.RoleID > 0 && roleControllerActions.Status == true)
                {
                    var result = _roleServices.AddRoleControllerAction(roleControllerActions);
                    
                    if (result == "OK")
                    {
                        TempData["SuccessMsg"] = $"Action đã được thêm vào Role thành công!";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Lỗi! " + result;
                    }
                }
            }
            return RedirectToAction("Roles");
        }
        #endregion


        #region ModuleControllers
        public IActionResult ModuleController()
        {
            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.ControllerLists = _moduleControllerServices.GetAllModuleController();
            return View(model);
        }
        public IActionResult CreateController(ModuleControllers module)
        {
            module.CreatedBy = User.Identity.Name;
            module.DateCreated = DateTime.Today;

            if (module.ControllerName != null)
            {
                var result = _moduleControllerServices.AddModuleController(module);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Thêm Controller: {module.ControllerName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("ModuleController");
            }
            TempData["ErrorMsg"] = "Lỗi! Không thể thêm Controller";
            return RedirectToAction("ModuleController");
        }

        [HttpGet]
        public IActionResult EditController(int id)
        {
            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.Controllers = _moduleControllerServices.GetModuleControllersByID(id);
            return PartialView("_EditModuleController", model);
        }

        [HttpPost]
        public IActionResult EditController(ModuleControllers module)
        {
            module.ModifiedBy = User.Identity.Name;
            module.DateModified = DateTime.Today;

            if (module.ControllerName != null)
            {
                var result = _moduleControllerServices.EditModuleController(module);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Thay đổi thông tin Controller: {module.ControllerName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("ModuleController");
            }
            TempData["ErrorMsg"] = "Lỗi! Không tìm thấy Controller";
            return RedirectToAction("ModuleController");
        }

        public IActionResult DeleteController(int id)
        {
            var module = _moduleControllerServices.GetModuleControllersByID(id);
            if (id > 0)
            {
                var result = _moduleControllerServices.DeleteModuleController(id);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Đã xóa Controller: {module.ControllerName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("ModuleController");
            }
            TempData["ErrorMsg"] = "Lỗi! Không tìm thấy Controller";
            return RedirectToAction("ModuleController");
        }
        #endregion


        #region ModuleActions
        public IActionResult ModuleAction()
        {
            var viewModel = new List<RoleControllerActions>();
            foreach (var action in _moduleActionServices.GetAllModuleActions())
            {
                ModuleControllers controllerData = new ModuleControllers();
                controllerData = _moduleControllerServices.GetModuleControllersByID(action.ControllerID);
                var data = new RoleControllerActions
                {
                    Controller_ID = controllerData.ControllerID,
                    ControllerName = controllerData.ControllerName,
                    ActionID = action.ActionID,
                    ActionName = action.ActionName,
                    Description = action.Description,
                    Status = action.Status,
                };
                viewModel.Add(data);
            }

            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.RoleControllerActionLists = viewModel;
            model.SelectController = new SelectList(_moduleControllerServices.GetAllModuleController(), "ControllerID", "ControllerName");
            return View(model);
        }

        public IActionResult CreateAction(ModuleActions module)
        {
            module.CreatedBy = User.Identity.Name;
            module.DateCreated = DateTime.Today;

            if (module.ActionName != null)
            {
                var result = _moduleActionServices.AddModuleActions(module);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Thêm Action: {module.ActionName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("ModuleAction");
            }
            TempData["ErrorMsg"] = "Lỗi! Không thể thêm Controller";
            return RedirectToAction("ModuleAction");
        }

        public IActionResult EditAction(int id)
        {
            var action = _moduleActionServices.GetModuleActionsByID(id);
            var controllerData = _moduleControllerServices.GetModuleControllersByID(action.ControllerID);
            var data = new RoleControllerActions
            {
                Controller_ID = controllerData.ControllerID,
                ControllerName = controllerData.ControllerName,
                ActionID = action.ActionID,
                ActionName = action.ActionName,
                Description = action.Description,
                Status = action.Status,
            };
            RoleControllerActionViewModel model = new RoleControllerActionViewModel();
            model.RoleControllerActions = data;
            model.SelectController = new SelectList(_moduleControllerServices.GetAllModuleController(), "ControllerID", "ControllerName");
            return PartialView("_EditModuleAction", model);
        }
        [HttpPost]
        public IActionResult EditAction(ModuleActions module)
        {
            module.ModifiedBy = User.Identity.Name;
            module.DateModified = DateTime.Today;

            if (module.ActionName != null)
            {
                var result = _moduleActionServices.EditModuleActions(module);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Thay đổi thông tin Action: {module.ActionName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("ModuleAction");
            }
            TempData["ErrorMsg"] = "Lỗi! Không tìm thấy Action";
            return RedirectToAction("ModuleAction");
        }

        public IActionResult DeleteAction(int id)
        {
            var module = _moduleActionServices.GetModuleActionsByID(id);
            if (id > 0)
            {
                var result = _moduleActionServices.DeleteModuleActions(id);
                if (result == "OK")
                {
                    TempData["SuccessMsg"] = $"Đã xóa Action: {module.ActionName} thành công!";
                }
                else
                {
                    TempData["ErrorMsg"] = "Lỗi! " + result;
                }
                return RedirectToAction("ModuleAction");
            }
            TempData["ErrorMsg"] = "Lỗi! Không tìm thấy Action";
            return RedirectToAction("ModuleAction");
        }
        #endregion
    }
}
