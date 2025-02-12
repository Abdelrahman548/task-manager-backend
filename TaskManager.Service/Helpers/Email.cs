using MimeKit;

namespace TaskManager.Service.Helpers
{
    public class Email
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string RecieverName { get; set; }
        public string RecieverEmail { get; set; }

        public Email(string senderName, string senderEmail, string recieverName, string recieverEmail)
        {
            SenderName = senderName;
            SenderEmail = senderEmail;
            RecieverName = recieverName;
            RecieverEmail = recieverEmail;
        }

        public MimeMessage PrepareEmailHtmlBody(string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(SenderName, SenderEmail));
            email.To.Add(new MailboxAddress(RecieverName, RecieverEmail));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };
            return email;
        }
        public MimeMessage PrepareEmailPlainBody(string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(SenderName, SenderEmail));
            email.To.Add(new MailboxAddress(RecieverName, RecieverEmail));
            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };
            return email;
        }
    }
}
