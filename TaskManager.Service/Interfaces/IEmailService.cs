using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.Helpers;

namespace TaskManager.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailWithHtmlBody(string recieverName, string recieverEmail, string subject, string body);
        Task SendEmailWithPlainBody(string recieverName, string recieverEmail, string subject, string body);
    }
}
