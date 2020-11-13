using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreIdentity.Context;
using NetCoreIdentity.CustomValidator;

namespace NetCoreIdentity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>();
            //// kayit ol vs alaninda hata mesajlarina takilmamak icin
            //services.AddIdentity<AppUser, AppRole>(opt =>
            //{
            //    // sifre ayarlari
            //    opt.Password.RequireDigit = false;
            //    opt.Password.RequireLowercase = false;
            //    opt.Password.RequiredLength = 3;
            //    opt.Password.RequireNonAlphanumeric = false;
            //    opt.Password.RequireUppercase = false;

            //    //Kullanici engelleme
            //    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            //    opt.Lockout.MaxFailedAccessAttempts = 5;
            //}).AddEntityFrameworkStores<IdentityContext>();


            ///Hata mesajlarini Turkcelestirmek icin yukaridaki kod blogu degistirilecegi icin yedek alarak
            /// bu islemi gerceklestirmek istedim. Bu sayede onceki ve sonraki kod farkliliklari gorulmektedir.
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                // isnotallowed durumu : email dogrulamasi koyulan bir sistemde
                // dogrulama islemi yapilana kadar sisteme girisini engelleme durumu
                opt.SignIn.RequireConfirmedEmail = true;


                //Kullanici engelleme
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            }).AddErrorDescriber<CustomIdentityValidator>().AddEntityFrameworkStores<IdentityContext>();


            services.AddControllersWithViews();

            // Cookie ayarlari
            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Home/Index";
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
            // PanelController icerisindeki Authorize attr kullanmak icin kullanilan middleware
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
