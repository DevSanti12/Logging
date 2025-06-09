using System;
using BrainstormSessions.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace BrainstormSessions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Email sink configuration
            var emailConnectionInfo = new EmailConnectionInfo
            {
                FromEmail = "santiago_leyva@epam.com",           // Sender email address
                ToEmail = "santiagoleyva2013@gmail.com",       // Recipient email address
                MailServer = "smtp.example.com",               // SMTP server address
                NetworkCredentials = new System.Net.NetworkCredential
                {
                    UserName = "your_smtp_username",           // SMTP username
                    Password = "your_smtp_password"            // SMTP password
                },
                EnableSsl = true,                              // Use SSL (secure connection)
                Port = 587,                                    // SMTP port (587 is common)
                EmailSubject = "Error Logs from BrainstormSessions" // Subject of the email
            };

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
