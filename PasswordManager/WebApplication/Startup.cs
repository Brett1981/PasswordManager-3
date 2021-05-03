using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class Startup
    {
        private string _connectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                        .AddDbContext<DataAccess.PasswordManagerContext>(options => options.UseSqlServer(_connectionString));
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = "PasswordManager.Session";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(DataAccess.AutomapperProfiles));
            services.AddRazorPages();
            services.AddControllers();
            services.AddTransient<DataAccess.Interfaces.IAccountRepository, DataAccess.AccountRepository>();
            services.AddTransient<DataAccess.Interfaces.IUserRepository, DataAccess.UserRepository>();
            services.AddControllersWithViews();
            services.AddMvc(options => options.EnableEndpointRouting = false);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _connectionString = Configuration.GetConnectionString("PasswordManager");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
             name: "route2",
             template: "statics",
             defaults: new { controller = "User", action = "Login" }

            );

                routes.MapRoute(
                   name: "route3",
                   template: "statics/SYears",
                   defaults: new { controller = "SYears", action = "Index" }
                );
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();

            //    endpoints.MapControllerRoute(name: "user",
            //    pattern: "User/Login",
            //    defaults: new { controller = "User", action = "Login" });

            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=User}/{action=Login}/{id?}");
            //});
        }
    }
}
