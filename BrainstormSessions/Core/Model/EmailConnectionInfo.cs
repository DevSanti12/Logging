using System.Net;

namespace BrainstormSessions.Core.Model
{
    internal class EmailConnectionInfo
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string MailServer { get; set; }
        public NetworkCredential NetworkCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string EmailSubject { get; set; }
    }
}