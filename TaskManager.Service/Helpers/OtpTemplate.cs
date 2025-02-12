using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.Helpers
{
    public static class OtpTemplate
    {
        public static string GetTemplate(string otpCode, int expirationTime, string companyName)
        {
            string htmlTemplate = "";
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string servicePath = Path.Combine(basePath, "..", "..", "..", "..", "TaskManager.Service");
                string templatePath = Path.Combine(servicePath, "Templates", "otp_template.html");
                htmlTemplate = File.ReadAllText(templatePath);
            }
            catch
            {

            }
            string emailBody = htmlTemplate.Replace("{{OTP_CODE}}", $"{otpCode}");
            emailBody = emailBody.Replace("{{OTP_EXPIRATION_TIME}}", $"{expirationTime} minutes");
            emailBody = emailBody.Replace("{{COMPANY_NAME}}", $"{companyName}");
            return emailBody;
        }
    }
}
