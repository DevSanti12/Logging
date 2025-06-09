using System;
using BrainstormSessions.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.EmailPickup;

namespace BrainstormSessions
{
    public class Program
    {
        public static void Main(string[] args)
        {


            try
            {
                Log.Logger = new LoggerConfiguration().
                    ReadFrom.Configuration(new ConfigurationBuilder().
                        AddJsonFile("appsettings.json") //read from appsettings to configure logger
                        .Build()).CreateLogger();
                Log.Information("Starting the application...");
                Log.Error("Testing email log creation in pickup folder...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensure all queued logs are flushed
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Hook Serilog into ASP.NET Core's logging system
                .ConfigureServices((context, services) =>
                {
                    // Register Serilog.ILogger globally
                    services.AddSingleton(Log.Logger); // This ensures Serilog.ILogger can be injected
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
