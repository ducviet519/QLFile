using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public interface IUnitOfWork
    {
        IReportListServices Report_List {get;} 
        IReportVersionServices Report_Version { get; }
        IReportSoftServices Report_Soft { get; }
        IReportDetailServices Report_Detail { get; }
        IGopYServices Report_GopY { get; }

        IReportURDServices DM_URDs { get; }
        IDepts DM_Depts { get; }
        ISoftwareServices DM_Softwares { get; }

        IRolesServices Auth_Roles { get; }
        IUserServices Auth_Users { get; }
        IModuleControllerServices Auth_Controllers { get; }
        IModuleActionServices Auth_Actions { get; }

        IGoogleDriveAPI GoogleDriveAPI { get; }
        IUploadFileServices UploadFile { get; }

        IBaoHiemTuNguyenServices BaoHiemTuNguyen { get; }
    }
}
