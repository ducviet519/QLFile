using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using WebTools.Models;
using Dapper;
using System;
using System.Linq;
using WebTools.Context;
using System.Threading.Tasks;

namespace WebTools.Services
{
    public class ReportVersionServices : IReportVersionServices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public ReportVersionServices(IConfiguration configuration)
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
        public async Task<string> DeleteReportVersionAsync(string IDPhienBan)
        {
            string result = "";
            var query = "DELETE FROM Report_version WHERE ID = @IDPhienBan";
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    var data = await dbConnection.ExecuteAsync(query, new{ IDPhienBan = IDPhienBan });
                    if (data != 0)
                    {
                        result = "DEL";
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

        //Lấy về danh sách Phiên bản sp_Report_Version_View
        public async Task<List<ReportVersion>> GetReportVersionAsync(string IdBieuMau)
        {
            List<ReportVersion> reportLists = new List<ReportVersion>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    reportLists = (await dbConnection.QueryAsync<ReportVersion>("sp_Report_Version_View",new{ IDBieuMau = IdBieuMau }, commandType: CommandType.StoredProcedure)).ToList();
                    dbConnection.Close();                   
                }
                return reportLists;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reportLists;
            }
        }

        //Thêm mới phiên bản sp_Report_Version_Add
        public async Task<string> InsertReportVersionAsync(ReportVersion reportVersion)
        {
            string result = "";
            DateTime? NgayBanHanh = null;
            if (!String.IsNullOrEmpty(reportVersion.NgayBanHanh))
            {
                NgayBanHanh = DateTime.ParseExact(reportVersion.NgayBanHanh, "dd/MM/yyyy", null);
            }
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    var data = await dbConnection.QueryAsync<ReportVersion>("sp_Report_Version_Add",
                        new
                        {
                            IDBieuMau = reportVersion.IDBieuMau,
                            NgayBH = NgayBanHanh,
                            FileLink = reportVersion.FileLink,
                            GhiChu = reportVersion.GhiChu,
                            PhienBan = reportVersion.PhienBan,
                            User = reportVersion.CreatedUser
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

        public async Task<string> UpdateReportVersionAsync(ReportVersion reportVersion)
        {
            string result = "";
            var query = "sp_Report_Version_Edit";
            DateTime? NgayBanHanh = null;
            if (!String.IsNullOrEmpty(reportVersion.NgayBanHanh))
            {
                NgayBanHanh = DateTime.ParseExact(reportVersion.NgayBanHanh, "dd/MM/yyyy", null);
            }
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    var data = await dbConnection.QueryAsync<ReportVersion>(query
                        ,new 
                        { 
                            IDPhienBan = reportVersion.IDPhienBan,
                            NgayBH = NgayBanHanh,
                            FileLink = reportVersion.FileLink,
                            GhiChu = reportVersion.GhiChu,
                            PhienBan = reportVersion.PhienBan,
                            User = reportVersion.CreatedUser

                        }
                        , commandType: CommandType.StoredProcedure);
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
    }
}
