using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Services;
using WebStore.Infrastructure.Services.Interfaces;

namespace WebStore
{
    public record Startup(IConfiguration Configuration)
    {
        //private IConfiguration Configuration { get; }

        //public Startup(IConfiguration Configuration)
        //{
        //    this.Configuration = Configuration;
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt => 
                opt.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            services.AddTransient<IProductData, InMemoryProductData>();

            //services.AddScoped<StoragePrinter>();
            //services.AddScoped<IPrinter, StoragePrinter>();
            //services.AddScoped<IMessagesStorage, ListMessageStorage>();
            //services.AddScoped<IPrinter, StoragePrinter2>();

            //services.AddMvc();

            services
               .AddControllersWithViews(
                    mvc =>
                    {
                        //mvc.Conventions.Add(new ActionDescriptionAttribute("123"));
                        mvc.Conventions.Add(new ApplicationConvention());
                    })
               .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, IServiceProvider services*/)
        {
            //var printer1 = services.GetRequiredService<IPrinter>();
            //var printer11 = services.GetRequiredService<StoragePrinter>();
            //var printer2 = services.GetRequiredService<IPrinter>();

            //using (var scope = services.CreateScope())
            //{
            //    var printer3 = scope.ServiceProvider.GetRequiredService<IPrinter>();
            //}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            //app.Map()
            //app.Use()

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/Greetings", async context =>
                {
                    await context.Response.WriteAsync(Configuration["Greetings"]);
                });

                //endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public interface IPrinter
    {
        void Print(string message);
    }

    class StoragePrinter : IPrinter, IDisposable
    {
        private readonly IMessagesStorage _Storage;

        public StoragePrinter(IMessagesStorage Storage) => _Storage = Storage;

        public void Print(string message) => _Storage.Add(message);

        public void Dispose()
        {
        }
    }

    class StoragePrinter2 : IPrinter, IDisposable
    {
        private readonly IMessagesStorage _Storage;

        public StoragePrinter2(IMessagesStorage Storage) => _Storage = Storage;

        public void Print(string message) => _Storage.Add(message);

        public void Dispose()
        {
        }
    }

    interface IMessagesStorage
    {
        void Add(string message);

        IEnumerable<(DateTime Time, string Message)> GetAll();
    }

    class ListMessageStorage : IMessagesStorage
    {
        private readonly List<(DateTime Time, string Message)> _Messages = new();

        public ListMessageStorage()
        {
            
        }

        public void Add(string message)
        {
            _Messages.Add((DateTime.Now, message));
        }

        public IEnumerable<(DateTime Time, string Message)> GetAll() => _Messages;
    }
}
