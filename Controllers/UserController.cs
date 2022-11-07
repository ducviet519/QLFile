using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Controllers
{  
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IRolesServices _roleServices;

        public UserController(IUserServices userServices, IRolesServices rolesServices)
        {
            _userServices = userServices;
            _roleServices = rolesServices;
        }

        public IActionResult Index()
        {     
            return View();
        }

        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }

        #region .

        private Users CheckLoginDomain(string domain, string username, string password)
        {
            try
            {
                var activeDirectoryHelper = new ActiveDirectoryHelper(domain);

                if (activeDirectoryHelper.IsAuthenticated(domain, username, password))
                {
                    var userprincipal = string.Format(@"{0}\{1}", domain, username);

                    using PrincipalContext context = new(ContextType.Domain, domain, userprincipal, password);

                    var userInfo = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userprincipal);

                    return new Users()
                    {
                        UserID = 0, //string.IsNullOrEmpty(userInfo.Description) ? 0 : int.Parse(userInfo.Description),
                        DisplayName = userInfo.DisplayName,
                        UserName = $@"{username.Trim().ToLower()}@{domain}",
                        Password = password,
                        Source = domain,
                        Email = userInfo.EmailAddress,
                        Status = (userInfo.Enabled ?? false)
                    };
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }
            return null;
        }

        #endregion

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid) 
            {
                TempData["Error"] = "Lỗi! Tài khoản hoặc mật khẩu không được bỏ trống";
                return View(login);
            }
            string domain = string.Empty;
            try
            {
                #region Lấy thông tin người dùng từ Windows Account
                var userDomain = login.UserName;
                domain = "bvta.vn";
                var userprincipal = $@"{domain}\{login.UserName}";
                using PrincipalContext context = new PrincipalContext(ContextType.Domain, domain, userprincipal, login.Password);
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, login.UserName);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin tài khoản.";
                }
                else
                {
                    Users userAccount = new Users()
                    {
                        UserID = 0, //string.IsNullOrEmpty(userInfo.Description) ? 0 : int.Parse(userInfo.Description),
                        DisplayName = user.DisplayName,
                        UserName = login.UserName.Trim().ToLower(),
                        Password = login.Password,
                        Source = domain,
                        Email = $@"{login.UserName.Trim().ToLower()}@{domain}",                       
                        Status = (user.Enabled ?? false)
                    };
                    //Ghi thông tin người dùng vào CSDL
                    _userServices.AddUser(userAccount);
                }
                #endregion

                #region Kiểm tra thông tin User và thêm ClaimUser
                string adPath = $"LDAP://{domain}";
                var ad_authenticate = new ActiveDirectoryHelper(adPath);
                //if (isAuthorized)
                if (ad_authenticate.IsAuthenticated(domain, login.UserName, login.Password))
                {
                    var UserLoginInfo = _userServices.FindByName(login.UserName);
                    var RoleInUser = _userServices.GetRoleInUser(UserLoginInfo.UserID);
                    var PermissionsInUser = _userServices.GetAllUserPermissions(UserLoginInfo.UserName);

                    var IP = HttpContext.Connection.RemoteIpAddress.ToString();
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, UserLoginInfo.DisplayName),
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(UserLoginInfo.UserID)),
                        new Claim(ClaimTypes.GroupSid, string.IsNullOrEmpty(domain) ? "local" : domain),
                        new Claim(ClaimTypes.Email, $@"{login.UserName.Trim().ToLower()}@{domain}" ?? ""),
                    };
                    foreach (var role in RoleInUser)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.RoleName));

                    }
                    foreach (var permission in PermissionsInUser)
                    {
                        claims.Add(new Claim("Permission", $"{permission.Permission}"));
                    }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        new AuthenticationProperties()
                        {
                            IsPersistent = login.RememberLogin
                        });
                    string returnUrl = Request.Form["ReturnUrl"];
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                #endregion

                TempData["Error"] = "Lỗi! Tài khoản hoặc Mật khẩu không chính xác";
                return View();
            }
            catch(Exception ex)
            {
                var errorMessage = ex.Message;
                TempData["Error"] = $"Lỗi! Tài khoản hoặc Mật khẩu không chính xác";
                return View();
            }
        }
      
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
