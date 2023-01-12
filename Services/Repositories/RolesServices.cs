using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class RolesServices : IRolesServices
    {
        #region Connection Database
        private readonly IConfiguration _configuration;

        public RolesServices(IConfiguration configuration)
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
        public string AddRoleControllerAction(RoleControllerActions roleControllerActions)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<RoleControllerActions>("sp_Roles",
                        new
                        {
                            ID = roleControllerActions.RoleID,
                            ControllerID = roleControllerActions.Controller_ID,
                            ActionID = roleControllerActions.ActionID,
                            Action = "AddRoleAction"
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

        public string AddRoles(Roles roles)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Roles>("sp_Roles",
                        new
                        {
                            Name = roles.RoleName,
                            Description = roles.Description,
                            Status = roles.Status,
                            User = roles.CreatedBy,
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

        public string DeleteRoleControllerAction(int id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<RoleControllerActions>("sp_Roles",
                        new
                        {
                            ID = id,
                            Action = "DeleteRoleAction"
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

        public string DeleteRoles(int? id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Roles>("sp_Roles",
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

        public string EditRoles(Roles roles)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<Roles>("sp_Roles",
                        new
                        {
                            ID = roles.RoleID,
                            Name = roles.RoleName,
                            Description = roles.Description,
                            Status = roles.Status,
                            User = roles.CreatedBy,
                            Action = "Edit"
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

        public Roles FindByName(string RoleName)
        {
            Roles roles = new Roles();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Roles>("sp_Roles", new
                    {
                        Name = RoleName,
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

        public List<Roles> GetAllRoles()
        {
            List<Roles> roles = new List<Roles>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Roles>("sp_Roles"
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

        public List<RoleControllerActions> GetRolePermissions(int id)
        {
            List<RoleControllerActions> roles = new List<RoleControllerActions>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<RoleControllerActions>("sp_Roles"
                        , new
                        {
                            ID = id,
                            Action = "GetRolePermissions"

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

        public Roles GetRolesByID(int? id)
        {
            Roles roles = new Roles();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roles = dbConnection.Query<Roles>("sp_Roles", new
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

        public bool IsRoleInControllerAction(int RoleID, int ControllerID, int ActionID)
        {
            List<RoleControllerActions> roleControllerActions = new List<RoleControllerActions>();
            var sql = "SELECT * FROM dbo.RoleControllers WHERE (RoleID = @RoleID) AND (ControllerID = @ControllerID) AND (ActionID = @ActionID)";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    roleControllerActions = dbConnection.Query<RoleControllerActions>(sql, new { RoleID = RoleID, ControllerID = ControllerID, ActionID = ActionID }).ToList();
                    dbConnection.Close();
                }
                if (roleControllerActions.Count > 0) { return true; }
                else { return false; }

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return false;
            }
        }


    }
}
