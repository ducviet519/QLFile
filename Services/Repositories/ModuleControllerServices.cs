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
    public class ModuleControllerServices : IModuleControllerServices
    {
        #region Connection Database
        
        private readonly IConfiguration _configuration;
        public ModuleControllerServices(IConfiguration configuration)
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

        public string AddModuleController(ModuleControllers moduleControllers)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ModuleControllers>("sp_ModuleControllers",
                        new
                        {
                            ID = "",
                            Name = moduleControllers.ControllerName,
                            Description = moduleControllers.Description,
                            Status = moduleControllers.Status,
                            User = moduleControllers.CreatedBy,
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

        public string DeleteModuleController(int id)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ModuleControllers>("sp_ModuleControllers",
                        new
                        {
                            ID = id,
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
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

        public string EditModuleController(ModuleControllers moduleControllers)
        {
            string result = "";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = dbConnection.Query<ModuleControllers>("sp_ModuleControllers",
                        new
                        {
                            ID = moduleControllers.ControllerID,
                            Name = moduleControllers.ControllerName,
                            Description = moduleControllers.Description,
                            Status = moduleControllers.Status,
                            User = moduleControllers.ModifiedBy,
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

        public List<ModuleControllers> GetAllModuleController()
        {
            List<ModuleControllers> modules = new List<ModuleControllers>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    modules = dbConnection.Query<ModuleControllers>("sp_ModuleControllers"
                        , new
                        {
                            ID = "",
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
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

        public ModuleControllers GetModuleControllersByID(int id)
        {
            ModuleControllers modules = new ModuleControllers();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    modules = dbConnection.Query<ModuleControllers>("sp_ModuleControllers"
                        , new
                        {
                            ID = id,
                            Name = "",
                            Description = "",
                            Status = "",
                            User = "",
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
