using System.Collections.Generic;
using WebTools.Models;
using WebTools.Context;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Services
{
    public class ReportURDServices:IReportURDServices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public ReportURDServices(IConfiguration configuration)
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
        public async Task<List<ReportURD>> GetAll_URDAsync()
        {
            List<ReportURD> reporURDs = new List<ReportURD>();
            var sql = "SELECT * FROM Report_URD";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reporURDs = (await dbConnection.QueryAsync<ReportURD>(sql)).ToList();
                    dbConnection.Close();
                }
                return reporURDs;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reporURDs;
            }
        }
    }
}
