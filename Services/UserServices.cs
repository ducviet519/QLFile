﻿using Dapper;
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
                            ID = "",
                            DisplayName = users.DisplayName,
                            UserName = users.UserName,
                            Password = users.Password,
                            Source = users.Source,
                            Email = users.Email,
                            Status = users.Status,
                            Role_ID = "",
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
                            DisplayName = "",
                            UserName = "",
                            Password = "",
                            Source = "",
                            Email = "",
                            Status = "",
                            Role_ID = "",
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


        public string EditUserRoles(UserRoles userRoles)
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
                            DisplayName = "",
                            UserName = "",
                            Password = "",
                            Source = "",
                            Email = "",
                            Status = "",
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
                            ID = "",
                            DisplayName = "",
                            UserName = "",
                            Password = "",
                            Source = "",
                            Email = "",
                            Status = "",
                            Role_ID = "",
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
                        DisplayName = "",
                        UserName = "",
                        Password = "",
                        Source = "",
                        Email = "",
                        Status = "",
                        Role_ID = "",
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
    }
}
