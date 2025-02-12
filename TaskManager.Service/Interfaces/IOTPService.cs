using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.Interfaces
{
    public interface IOTPService
    {
        string GenerateOTP();
        string GetBodyTemplate(string otp, int time, string companyName);
        Task SendOTPEmailHtmlBody(string recieverName, string recieverEmail, string subject, string body);
        Task SendOTPEmailPlainBody(string recieverName, string recieverEmail, string subject, string body);
    }
}
