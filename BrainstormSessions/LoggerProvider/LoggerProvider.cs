using Serilog;

namespace BrainstormSessions.LoggerProvider
{
    public static class LoggerProvider
    {
        // Shared Serilog logger instance
        public static ILogger SharedLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()           // Enable DEBUG level and above
            .WriteTo.Log4Net()             // Forward logs to Log4Net
            .CreateLogger();
    }
}
