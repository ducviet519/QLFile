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
using WebTools.Models.Entities;

namespace WebTools.Services

{
    public class ReportListServices : IReportListServices
    {
        #region Connection String
        private readonly IConfiguration _configuration;
        public ReportListServices(IConfiguration configuration)
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
        public async Task<ReportList> GetReportByIDAsync(string id)
        {
            ReportList reports = new ReportList();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reports = (await dbConnection.QueryAsync<ReportList>("sp_Report_GetByID", new { IDBieuMau = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                    dbConnection.Close();
                }
                return reports;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return reports;
            }
        }
        //Thực thi StoredProcedure sp_Report_List lấy về danh sách Biểu mẫu
        public async Task<List<ReportList>> GetReportListAsync()
        {
            List<ReportList> reportLists = new List<ReportList>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reportLists = (await dbConnection.QueryAsync<ReportList>("sp_Report_List", commandType: CommandType.StoredProcedure)).ToList();
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
        //Thực thi StoredProcedure sp_Report_New thêm mới Biểu mẫu
        public async Task<string> InsertReportListAsync(ReportList reportList)
        {
            string result = "";
            DateTime? NgayBanHanh = null;
            if (!String.IsNullOrEmpty(reportList.NgayBanHanh))
            {
                NgayBanHanh = DateTime.ParseExact(reportList.NgayBanHanh, "dd/MM/yyyy", null);
            }
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.QueryAsync<ReportList>("sp_Report_New",
                        new
                        {
                            TenBM = reportList.TenBM,
                            MaBM = reportList.MaBM,
                            NgayBH = NgayBanHanh,
                            FileLink = reportList.FileLink,
                            GhiChu = reportList.GhiChu,
                            KhoaPhongSD = reportList.KhoaPhong,
                            PhienBan = reportList.PhienBan,
                            TheLoai = reportList.postTheLoai,
                            User = reportList.CreatedUser
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

        public async Task<List<ReportList>> SearchReportListAsync(string SearchURD = null, string searchString = null, string searchDate = null, string searchTrangThaiSD = null, string searchTrangThaiPM = null)
        {
            List<ReportList> reportLists = new List<ReportList>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    reportLists = (await dbConnection.QueryAsync<ReportList>("sp_Report_List", new
                    {
                        search = searchString,
                        NgayBH = searchDate,
                        TrangThaiSD = searchTrangThaiSD,
                        TrangThaiPM = searchTrangThaiPM,
                        URD = SearchURD
                    }, commandType: CommandType.StoredProcedure)).ToList();
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


        public async Task<string> UpdateReportListAsync(ReportList reportList)
        {
            string result = "";
            DateTime? NgayBanHanh = null;
            if (!String.IsNullOrEmpty(reportList.NgayBanHanh))
            {
                NgayBanHanh = DateTime.ParseExact(reportList.NgayBanHanh, "dd/MM/yyyy", null);
            }
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.QueryAsync<ReportList>("sp_Report_Edit",
                        new
                        {
                            IDBieuMau = reportList.IDBieuMau,
                            IDPhienBan = reportList.IDPhienBan,
                            TenBM = reportList.TenBM,
                            MaBM = reportList.MaBM,
                            NgayBH = NgayBanHanh,
                            FileLink = reportList.FileLink,
                            GhiChu = reportList.GhiChu,
                            KhoaPhongSD = reportList.KhoaPhong,
                            PhienBan = reportList.PhienBan,
                            TheLoai = reportList.postTheLoai,
                            User = reportList.CreatedUser
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
        public async Task<List<ReportList>> SearchReportNameAsync(string SearchURD = null, List<GoogleDriveFile> Table = null, string searchDate = null, string searchTrangThaiSD = null, string searchTrangThaiPM = null)
        {
            List<ReportList> reportLists = new List<ReportList>();
            var fileTable = new List<GoogleDriveFile>();
            foreach (var file in Table)
            {
                fileTable.Add(new GoogleDriveFile { FileName = file.FileName });
            }
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    reportLists = (await dbConnection.QueryAsync<ReportList>("sp_Report_List_Content",
                        new
                        {
                            FileName = fileTable.AsTableValuedParameter("dbo.ReportFileName",
                            new[] { "FileName" }),
                            URD = SearchURD,
                            //TrangThaiSD = searchTrangThaiSD,
                            //TrangThaiPM = searchTrangThaiPM
                        },
                        commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<string> DeleteReportListAsync(string IDBieuMau, string IDPhienBan)
        {
            string result = String.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var data = await dbConnection.QueryAsync<ReportList>("sp_Report_Delete",
                        new
                        {
                            IDBieuMau = IDBieuMau,
                            IDPhienBan = IDPhienBan,
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

        public async Task<List<ExportExcel>> ExcelData()
        {
            List<ExportExcel> data = new List<ExportExcel>();
            
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();
                    data = (await dbConnection.QueryAsync<ExportExcel>("sp_Report_Excel",
                        new { }, commandType: CommandType.StoredProcedure)).ToList();
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
