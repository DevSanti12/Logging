using System.Net;
using Serilog.Sinks.Email;

namespace BrainstormSessions.Core.Model
{
    public class CustomEmailConnectionInfo
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string MailServer { get; set; }
        public NetworkCredential NetworkCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string EmailSubject { get; set; }
        public string PickupDirectoryLocation { get; set; }
    }
}