using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;
using MailKit.Net.Smtp;

namespace TaskManager.Service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions emailOptions;

        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string AppPassword { get; set; }

        private EmailService(string server, int port, bool useSSL, string appPassword, EmailOptions emailOptions)
        {
            Server = server;
            AppPassword = appPassword;
            this.emailOptions = emailOptions;
            Port = port;
            UseSSL = useSSL;
        }
        public static EmailService CreateWithSSL(EmailOptions emailOptions)
        {
            return new(emailOptions.SmtpServer, emailOptions.PortSSL, true, emailOptions.AppPassword, emailOptions);
        }
        public static EmailService CreateWithoutSSL(EmailOptions emailOptions)
        {
            return new(emailOptions.SmtpServer, emailOptions.Port, false, emailOptions.AppPassword, emailOptions);
        }
        public void SendEmailWithHtmlBody(string recieverName, string recieverEmail, string subject, string body)
        {
            Email email = new Email(emailOptions.SenderName, emailOptions.SenderEmail, recieverName, recieverEmail);
            var preparedEmail = email.PrepareEmailHtmlBody(subject, body);
            using (var client = new SmtpClient())
            {
                client.Connect(Server, Port, UseSSL);
                client.Authenticate(email.SenderEmail, AppPassword);
                client.Send(preparedEmail);
                client.Disconnect(true);
            }
        }
        public void SendEmailWithPlainBody(string recieverName, string recieverEmail, string subject, string body)
        {
            Email email = new Email(emailOptions.SenderName, emailOptions.SenderEmail, recieverName, recieverEmail);
            var preparedEmail = email.PrepareEmailPlainBody(subject, body);
            using (var client = new SmtpClient())
            {
                client.Connect(Server, Port, UseSSL);
                client.Authenticate(email.SenderEmail, AppPassword);
                client.Send(preparedEmail);
                client.Disconnect(true);
            }
        }
    }
}
