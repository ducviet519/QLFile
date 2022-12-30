using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IReportListServices Report_List { get; }
        public IReportVersionServices Report_Version { get; }
        public IReportSoftServices Report_Soft { get; }
        public IReportDetailServices Report_Detail { get; }
        public IReportURDServices DM_URDs { get; }
        public IDepts DM_Depts { get; }
        public ISoftwareServices DM_Softwares { get; }
        public IRolesServices Auth_Roles { get; }
        public IUserServices Auth_Users { get; }
        public IModuleControllerServices Auth_Controllers { get; }
        public IModuleActionServices Auth_Actions { get; }
        public IGoogleDriveAPI GoogleDriveAPI { get; }
        public IBaoHiemTuNguyenServices BaoHiemTuNguyen { get; }
        public IUploadFileServices UploadFile { get; }
        public IGopYServices Report_GopY { get; }

        public UnitOfWork
            (
                IReportListServices _Report_List,
                IReportVersionServices _Report_Version,
                IReportSoftServices _Report_Soft,
                IReportDetailServices _Report_Detail,
                IGopYServices _Report_GopY,

                IReportURDServices _DM_URDs,
                IDepts _DM_Depts,
                ISoftwareServices _DM_Softwares,

                IRolesServices _Auth_Roles,
                IUserServices _Auth_Users,
                IModuleControllerServices _Auth_Controllers,
                IModuleActionServices _Auth_Actions,

                IGoogleDriveAPI _GoogleDriveAPI,               
                IUploadFileServices _UploadFile,

                IBaoHiemTuNguyenServices _BaoHiemTuNguyen
            )
        {
            Report_List = _Report_List;
            Report_Version = _Report_Version;
            Report_Soft = _Report_Soft;
            Report_Detail = _Report_Detail;
            Report_GopY = _Report_GopY;

            DM_URDs = _DM_URDs;
            DM_Depts = _DM_Depts;
            DM_Softwares = _DM_Softwares;

            Auth_Roles = _Auth_Roles;
            Auth_Users = _Auth_Users;
            Auth_Controllers = _Auth_Controllers;
            Auth_Actions = _Auth_Actions;
            
            GoogleDriveAPI = _GoogleDriveAPI;
            UploadFile = _UploadFile;

            BaoHiemTuNguyen = _BaoHiemTuNguyen;            
        }

    }
}
