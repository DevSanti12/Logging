using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using log4net;

namespace BrainstormSessions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetExecutingAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            CreateHostBuilder(args).Build().Run();
            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.File(new JsonFormatter(), @"c:\temp\brainstormSession.log",
            //                    shared: true)
            //    .CreateLogger();

            //try
            //{
            //    Log.Information("Starting web application");

            //    var builder = WebApplication.CreateBuilder(args);
            //    builder.Services.AddSerilog(); // <-- Add this line

            //    var app = builder.Build();
            //    app.MapGet("/", () => "Hello World!");

            //    app.Run();
            //}
            //catch (Exception ex)
            //{
            //    Log.Fatal(ex, "Application terminated unexpectedly");
            //}
            //finally
            //{
            //    Log.CloseAndFlush();
            //}
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostContext, services) =>
             {
                 // Load log4net configuration from log4net.config file
                 XmlConfigurator.Configure(new FileInfo("log4net.config"));

                 // Register Log4Net to be used with ASP.NET logging
                 Log.Logger = new LoggerConfiguration()
                     .WriteTo.Log4Net() // Forward Serilog logs to Log4Net
                     .CreateLogger();

                 services.AddSingleton(Log.Logger); // Add logger to DI if needed
             })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
