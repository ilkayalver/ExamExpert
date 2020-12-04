using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamExpert.Bussinees.Interfaces;
using ExamExpert.Bussinees.Services;
using ExamExpert.Data;
using ExamExpert.Data.Entities;
using ExamExpert.Data.Interfaces;
using ExamExpert.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExamExpert.WebApp
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
            var connectionSQLiteDB = Configuration.GetConnectionString("SQLiteDB");
            services.AddDbContext<ExamExpertSQLLiteDbContext>(opt => opt.UseSqlite(connectionSQLiteDB));

            //Write here Dependency Injection Codes
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IWiredHtmlAgilityService, WiredHtmlAgilityService>();


            services.AddIdentity<User, Role>()
               .AddRoles<Role>()
               .AddRoleManager<RoleManager<Role>>()
               .AddEntityFrameworkStores<ExamExpertSQLLiteDbContext>()
               .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.Configure<IdentityOptions>(options =>
            {

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 4;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddSession();

            services.AddControllersWithViews()
               .AddRazorRuntimeCompilation()
               .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
