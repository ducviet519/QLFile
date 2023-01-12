using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entity;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class DeptsServices : IDepts
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public DeptsServices(IConfiguration configuration)
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
        public async Task<List<Depts>> GetAll_DeptsAsync()
        {
            List<Depts> reporDepts = new List<Depts>();
            var sql = "SELECT * FROM Depts";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reporDepts = (await dbConnection.QueryAsync<Depts>(sql)).ToList();
                    dbConnection.Close();
                }
                return reporDepts;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reporDepts;
            }
        }
    }
}
