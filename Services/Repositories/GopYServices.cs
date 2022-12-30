using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class GopYServices : IGopYServices
    {
        #region Connection Database
        private readonly IConfiguration _configuration;
        public GopYServices(IConfiguration configuration)
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
        public async Task<string> ThemGopY(string IDBieuMau, string noidung, string user)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync("sp_Report_GopY",
                        new
                        {
                            IDBieuMau = IDBieuMau,
                            noidung = noidung,
                            user = user
                        },
                        commandType: CommandType.StoredProcedure);
                    if (data > 0)
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
    }
}
