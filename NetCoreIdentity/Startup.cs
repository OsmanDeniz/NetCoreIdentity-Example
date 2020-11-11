using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreIdentity.Context;

namespace NetCoreIdentity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>();
            // kayit ol vs alaninda hata mesajlarina takilmamak icin
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                // sifre ayarlari
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

                //Kullanici engelleme
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<IdentityContext>();
            services.AddControllersWithViews();

            // Cookie ayarlari
            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Name = "LoginCookie";
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromDays(7);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
