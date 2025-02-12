using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class OTPService : IOTPService
    {
        private readonly IEmailService emailService;

        public OTPService(IEmailService emailService) 
        {
            this.emailService = emailService;
        }
        public string GenerateOTP()
        {
            return OTPGenerator.GenerateOTP();
        }

        public string GetBodyTemplate(string otp, int time, string companyName)
        {
            return OtpTemplate.GetTemplate(otp, time, companyName);
        }

        public async Task SendOTPEmailHtmlBody(string recieverName, string recieverEmail, string subject, string body)
        {
            await emailService.SendEmailWithHtmlBody(recieverName, recieverEmail, subject, body);
        }

        public async Task SendOTPEmailPlainBody(string recieverName, string recieverEmail, string subject, string body)
        {
            await emailService.SendEmailWithPlainBody(recieverName, recieverEmail, subject, body);
        }
    }
}
