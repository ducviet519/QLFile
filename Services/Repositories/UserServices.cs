using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class UserServices : IUserServices
    {
        #region Connection Database
        private readonly IConfiguration _configuration;

        public UserServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("ToolsDB");
            provideName = "System.Data.SqlClient";
        }
        public string ConnectionString { get; }
        public string provideName { get; }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }
        #endregion

        public string AddUser(Users users)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Users>("sp_Users",
                        new
                        {
                            DisplayName = users.DisplayName,
                            UserName = users.UserName,
                            Password = users.Password,
                            Source = users.Source,
                            Email = users.Email,
                            Status = users.Status,
                            Action = "Add"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public string AddUserRoles(string UserName, string RoleName)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Users>("sp_Users",
                        new
                        {
                            UserName = UserName,
                            RoleName = RoleName,
                            Action = "AddUserRoles"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public string Delete(int id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Users>("sp_Users",
                        new
                        {
                            ID = id,
                            Action = "Delete"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public string DeleteRoleInUser(int id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Users>("sp_Users",
                        new
                        {
                            ID = id,
                            Action = "DeleteRoleInUser"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public string AddUserRolesByID(UserRoles userRoles)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Users>("sp_Users",
                        new
                        {
                            ID = userRoles.UserID,
                            Role_ID = userRoles.RoleID,
                            Action = "AddUserRoles"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public Users FindByName(string userName)
        {
            Users roles = new Users();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Users>("sp_Users", new
                    {
                        UserName = userName,
                        Action = "FindByName"
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    dbConnection.Close();
                }
                return roles;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return roles;
            }
        }

        public List<Users> GetAllUsers()
        {
            List<Users> roles = new List<Users>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Users>("sp_Users"
                        , new
                        {
                            Action = "GetAll"

                        }
                        , commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                }
                return roles;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return roles;
            }
        }

        public List<Roles> GetRoleInUser(string user)
        {
            List<Roles> roles = new List<Roles>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Roles>("sp_Users"
                        , new
                        {
                            UserName = user,
                            Action = "GetRoleInUser"

                        }
                        , commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                }
                return roles;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return roles;
            }
        }

        public Users GetUsersByID(int? id)
        {
            Users roles = new Users();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Users>("sp_Users", new
                    {
                        ID = id,
                        Action = "GetByID"
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    dbConnection.Close();
                }
                return roles;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return roles;
            }
        }

        public bool IsUserInRole(int UserID, int RoleID)
        {
            List<UsersViewModel> userInRole = new List<UsersViewModel>();
            var sql = "SELECT * FROM dbo.UserRoles WHERE (UserID = @UserID) AND (RoleID = @RoleID)";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    userInRole = dbConnection.Query<UsersViewModel>(sql, new { UserID = UserID, RoleID = RoleID }).ToList();
                    dbConnection.Close();
                }
                if (userInRole.Count > 0) { return true; }
                else { return false; }
                    
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return false;
            }
        }

        public UserPermissions RenderPermissions(int ControllerID, int ActionID)
        {
            UserPermissions permissions = new UserPermissions();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    permissions = dbConnection.Query<UserPermissions>("sp_Users", new
                    {
                        Controller_ID = ControllerID,
                        Action_ID = ActionID,
                        Action = "RenderPermissions"
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    dbConnection.Close();
                }
                return permissions;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return permissions;
            }
        }

        public string AddUserPermissions(UserPermissions userPermissions)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<UserPermissions>("sp_Users",
                        new
                        {
                            UserName = userPermissions.UserName,
                            Permission = userPermissions.Permission,
                            Controller_ID = userPermissions.ControllerID,
                            Action_ID = userPermissions.ActionID,
                            Action = "AddUserPermissions"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public string DeleteUserPermissions(string UserName)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Users>("sp_Users",
                        new
                        {
                            UserName = UserName,
                            Action = "DeleteUserPermissions"
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data != null)
                    {
                        result = "OK";
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }

        public async Task<List<UserPermissions>> GetAllUserPermissions(string user)
        {
            List<UserPermissions> data = new List<UserPermissions>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<UserPermissions>("sp_Users", new
                    {
                        UserName = user,
                        Action = "GetUserPermissions"

                    }, commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }

        public async Task<List<UserPermissions>> GetDefaultPermissions(string roleName)
        {
            List<UserPermissions> data = new List<UserPermissions>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<UserPermissions>("sp_Users", new
                    {
                        RoleName = roleName,
                        Action = "GetRolePermissions"

                    }, commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();
                }
                return data;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return data;
            }
        }
    }
}
