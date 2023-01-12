using Microsoft.Extensions.DependencyInjection;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IReportListServices, ReportListServices>();
            services.AddTransient<IReportVersionServices, ReportVersionServices>();
            services.AddTransient<IReportSoftServices, ReportSoftServices>();
            services.AddTransient<IReportDetailServices, ReportDetailServices>();
            services.AddTransient<IReportURDServices, ReportURDServices>();
            services.AddTransient<IDepts, DeptsServices>();
            services.AddTransient<ISoftwareServices, SoftwareServices>();
            services.AddTransient<IRolesServices, RolesServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IModuleControllerServices, ModuleControllerServices>();
            services.AddTransient<IModuleActionServices, ModuleActionServices>();
            services.AddTransient<IGoogleDriveAPI, GoogleDriveAPI>();
            services.AddTransient<IBaoHiemTuNguyenServices, BaoHiemTuNguyenServices>();
            services.AddTransient<IUploadFileServices, UploadFileServices>();
            services.AddTransient<IGopYServices, GopYServices>();
        }
    }
}
