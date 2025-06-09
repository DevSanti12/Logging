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
            // Configure Serilog globally
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // Log Debug and above
                .WriteTo.Console()    // Writes logs to console for development convenience
                .WriteTo.File("Logs/log-.txt",  // Writes logs to rolling files daily
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();

            try
            {
                Log.Information("Starting the application...");
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
