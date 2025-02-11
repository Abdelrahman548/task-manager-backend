using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.Helpers
{
    public static class OTPGenerator
    {
        public static string GenerateOTP(int length = 6)
        {
            var random = new Random();
            return random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length)).ToString();
        }
    }
}
