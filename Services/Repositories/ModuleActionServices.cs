using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class ModuleActionServices : IModuleActionServices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public ModuleActionServices(IConfiguration configuration)
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

        public string AddModuleActions(ModuleActions moduleActions)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ModuleActions>("sp_ModuleActions",
                        new
                        {
                            ID = "",
                            Name = moduleActions.ActionName,
                            Description = moduleActions.Description,
                            Status = moduleActions.Status,
                            User = moduleActions.CreatedBy,
                            ControllerID = moduleActions.ControllerID,
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

        public string DeleteModuleActions(int id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ModuleActions>("sp_ModuleActions",
                        new
                        {
                            ID = id,
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
                            ControllerID = "",
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

        public string EditModuleActions(ModuleActions moduleActions)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ModuleActions>("sp_ModuleActions",
                        new
                        {
                            ID = moduleActions.ActionID,
                            Name = moduleActions.ActionName,
                            Description = moduleActions.Description,
                            Status = moduleActions.Status,
                            User = moduleActions.ModifiedBy,
                            ControllerID = moduleActions.ControllerID,
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

        public List<ModuleActions> GetActionsInController(int controllerID)
        {
            List<ModuleActions> actionInController = new List<ModuleActions>();
            var sql = "SELECT ID AS ActionID, Name AS ActionName FROM dbo.ModuleActions WHERE ControllerID = @ControllerID";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    actionInController = dbConnection.Query<ModuleActions>(sql,new { ControllerID = controllerID }, commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                }
                return actionInController;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return actionInController;
            }
        }

        public List<ModuleActions> GetAllModuleActions()
        {
            List<ModuleActions> modules = new List<ModuleActions>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    modules = dbConnection.Query<ModuleActions>("sp_ModuleActions"
                        , new
                        {
                            ID = "",
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
                            ControllerID = "",
                            Action = "GetAll"

                        }
                        , commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                }
                return modules;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return modules;
            }
        }

        public ModuleActions GetModuleActionsByID(int id)
        {
            ModuleActions modules = new ModuleActions();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    modules = dbConnection.Query<ModuleActions>("sp_ModuleActions"
                        , new
                        {
                            ID = id,
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
                            ControllerID = "",
                            Action = "GetByID"

                        }
                        , commandType: CommandType.StoredProcedure).FirstOrDefault();
                    dbConnection.Close();
                }
                return modules;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return modules;
            }
        }
    }
}
