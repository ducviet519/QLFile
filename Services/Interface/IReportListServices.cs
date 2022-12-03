using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models;
using WebTools.Models.Entities;

namespace WebTools.Services
{
    public interface IReportListServices
    {
        public Task<List<ReportList>> GetReportListAsync();

        public Task<ReportList> GetReportByIDAsync(string id);
        public Task<string> InsertReportListAsync(ReportList reportList);
        public Task<string> UpdateReportListAsync(ReportList reportList);
        public Task<string> DeleteReportListAsync(string IDBieuMau, string IDPhienBan);
        public Task<List<ReportList>> SearchReportListAsync(string SearchURD = null, string searchString = null, string searchDate = null, string searchTrangThaiSD = null, string searchTrangThaiPM = null);
        public Task<List<ReportList>> SearchReportNameAsync(string SearchURD = null, List<GoogleDriveFile> Table = null, string searchDate = null, string searchTrangThaiSD = null, string searchTrangThaiPM = null);
    }
}
