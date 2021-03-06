using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Hubs;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Logger;
using WebStore.Services.Data;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
               .AddIdentityWebStoreWebAPIClients()
               .AddDefaultTokenProviders();

            //services.AddIdentityWebStoreWebAPIClients();

            //#region Identity stores custom implementations

            //services.AddTransient<IUserStore<User>, UsersClient>();
            //services.AddTransient<IUserRoleStore<User>, UsersClient>();
            //services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            //services.AddTransient<IUserEmailStore<User>, UsersClient>();
            //services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            //services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            //services.AddTransient<IUserClaimStore<User>, UsersClient>();
            //services.AddTransient<IUserLoginStore<User>, UsersClient>();

            //services.AddTransient<IRoleStore<Role>, RolesClient>();

            //#endregion

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "GB.WebStore";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddTransient<IEmployeesData, EmployeesClient>();
            services.AddScoped<IProductData, ProductsClient>();
            //services.AddScoped<ICartServices, InCookiesCartService>();
            services.AddScoped<ICartServices, CartService>();
            services.AddScoped<ICartStore, InCookiesCartStore>();
            services.AddScoped<IOrderService, OrdersClient>();
            services.AddScoped<IValuesService, ValuesClient>();

            services
               .AddControllersWithViews(
                    mvc =>
                    {
                        mvc.Conventions.Add(new ApplicationConvention());
                    })
               .AddRazorRuntimeCompilation();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            //log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");

                endpoints.MapGet("/Greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
