using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Context;
using WebTools.Models;

namespace WebTools.Services
{
    public class ReportDetailServices : IReportDetailServices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public ReportDetailServices(IConfiguration configuration)
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
        public async Task<List<ReportDetail>> GetReportDetailAsync(string IdBieuMau)
        {
            List<ReportDetail> reportLists = new List<ReportDetail>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if(dbConnection.State==ConnectionState.Closed)
                    dbConnection.Open();
                    reportLists = (await dbConnection.QueryAsync<ReportDetail>("sp_Report_Detail_View", new { IDBieuMau = IdBieuMau }, commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<string> InsertReportDetailAsync(ReportDetail reportDetail)
        {
            string result = "";
            var detailTable = new List<ReportDetail>
            {
                new ReportDetail
                {
                    IDBieuMau = reportDetail.IDBieuMau,
                    IDPhienBan = reportDetail.IDPhienBan,
                    KhoaPhong = reportDetail.KhoaPhong,
                    TrangThai = reportDetail.TrangThai,
                    GhiChu = reportDetail.GhiChu
                }
            };
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    var data = await dbConnection.QueryAsync<ReportDetail>("sp_Report_Detail_Add",
                        new
                        {
                            ReportDetail = detailTable.AsTableValuedParameter("dbo.ReportDetail",
                            new[] { "IDBieuMau", "IDPhienBan", "KhoaPhong", "TrangThai","GhiChu" }),
                            User = reportDetail.User
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

    }
}
