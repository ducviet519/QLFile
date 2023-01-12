using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class BaoHiemTuNguyenServices : IBaoHiemTuNguyenServices
    {
        #region Connection Database

        private readonly IConfiguration _configuration;
        public BaoHiemTuNguyenServices(IConfiguration configuration)
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

        public async Task<List<DanhSachKhachHangBHTN>> GetAllKhanhHangThanhToanBHTN()
        {
            List<DanhSachKhachHangBHTN> data = new List<DanhSachKhachHangBHTN>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhSachKhachHangBHTN>("sp_BHTN_List", commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<List<DanhSachKhachHangBHTN>> SearchKhanhHangThanhToanBHTN(string mabn, string loai = "", string ngayBD = "", string ngayKT = "")
        {
            if (!String.IsNullOrEmpty(ngayBD)){
                ngayBD = DateTime.Parse(ngayBD, new CultureInfo("vi-VN", true)).ToString("yyyyMMdd");
            }
            if (!String.IsNullOrEmpty(ngayKT))
            {
                ngayKT = DateTime.Parse(ngayKT, new CultureInfo("vi-VN", true)).ToString("yyyyMMdd");
            }
            List<DanhSachKhachHangBHTN> data = new List<DanhSachKhachHangBHTN>();

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    data = (await dbConnection.QueryAsync<DanhSachKhachHangBHTN>("sp_BHTN_List", new {
                        NgayBD = ngayBD,
                        NgayKT = ngayKT,
                        mabn = mabn,
                        Loai = loai,
                    }, commandType: CommandType.StoredProcedure)).ToList();
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
