using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using OCMDomain.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OCM
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
            //Add identity Services

            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<OCMContext>().AddDefaultTokenProviders();
            services.AddIdentity<Users, IdentityRole>(options =>
            {
                // Configure identity options here.
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
           .AddEntityFrameworkStores<OCMContext>().AddDefaultTokenProviders();
            services.AddDbContext<OCMContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OCMContextConnection")));
            services.AddMvc();
            services.AddScoped<EmailService>();

           

            //services.AddSingleton<IHostEnvironment>(env);

            services.AddRazorPages();
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddSessionStateTempDataProvider();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession();

            

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<OCMContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Public/Security/AccessDenied";
            });
            // services.AddSingleton<OCMContext>();

            //services.AddScoped<OCMContext>();


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
            }
          
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapAreaControllerRoute(
                //   name: "Administration",
                //   areaName: "Administration",
                //   pattern: "Administration/{controller=Home}/{action=Index}");

                //endpoints.MapControllerRoute(
                //    name: "Finance",
                //    pattern: "{controller=Finance}/{action=Index}/{id?}");

                //endpoints.MapControllerRoute(
                //  name: "default",
                //  pattern: "{area=Public}/{controller=Public}/{action=Index}/{id?}");

                //endpoints.MapRazorPages();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{area=Public}/{controller=Public}/{action=Index}/{id?}");
                });
            });
        }
    }
}
