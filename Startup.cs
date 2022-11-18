using GleamTech.AspNet.Core;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebTools.Authorization;
using WebTools.Context;
using WebTools.Models.Entities;
using WebTools.Router;
using WebTools.Services;
using WebTools.Services.Interface;

namespace WebTools
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddControllersWithViews();
            services.AddAuthentication(
            
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/Denied";
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnSigningIn = async context =>
                        {
                            await Task.CompletedTask;
                        },
                        OnSignedIn = async context =>
                        {
                            await Task.CompletedTask;
                        },
                        OnValidatePrincipal = async context =>
                        {
                            await Task.CompletedTask;
                        }
                    };
                });
            
            services.AddScoped<IReportListServices, ReportListServices>();
            services.AddScoped<IReportVersionServices, ReportVersionServices>();
            services.AddScoped<IReportSoftServices, ReportSoftServices>();
            services.AddScoped<IReportDetailServices, ReportDetailServices>();
            services.AddScoped<IReportURDServices, ReportURDServices>();
            services.AddScoped<IDepts, DeptsServices>();
            services.AddScoped<ISoftwareServices, SoftwareServices>();
            services.AddScoped<IRolesServices, RolesServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IModuleControllerServices, ModuleControllerServices>();
            services.AddScoped<IModuleActionServices, ModuleActionServices>();
            services.AddScoped<IGoogleDriveAPI, GoogleDriveAPI>();
            services.AddScoped<IBaoHiemTuNguyenServices, BaoHiemTuNguyenServices>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUploadFileServices, UploadFileServices>();
            
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ToolsDB")));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddGleamTech();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //Register GleamTech to the ASP.NET Core HTTP request pipeline.
            //----------------------
            app.UseGleamTech();
            //----------------------
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //var rewrite = new RewriteOptions();
            //    rewrite.Rules.Add(new CustomRule());
            //app.UseRewriter(rewrite);

            var options = new RewriteOptions()
            .AddRedirectToHttps();
            app.UseRewriter(options);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
