using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace WebStore.ServiceHosting
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging((host, log) => log
                //   //.ClearProviders()
                //   //.AddEventLog()
                //   //.AddConsole(opt => opt.IncludeScopes = true)
                //   //.AddFilter/*<ConsoleLoggerProvider>*/("Microsoft.Hosting", LogLevel.Error)
                //   //.AddFilter((category, level) => !(category.StartsWith("Microsoft") && level >= LogLevel.Warning))
                //)
               .ConfigureWebHostDefaults(host => host
                   .UseStartup<Startup>()
                )
               .UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
                   .MinimumLevel.Debug()
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                   .Enrich.FromLogContext()
                   .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
                   .WriteTo.RollingFile($@"Logs/WebStore.Services[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
                   .WriteTo.File(new JsonFormatter(",", true), $@"Logs/WebStore.Services[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
                   .WriteTo.Seq(host.Configuration["seq"]))
        ;
    }
}
