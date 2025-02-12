using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Entities
{
    public class OTPVerification : Entity
    {
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        public string HashedOTP { get; set; } = string.Empty;
        public DateTime ExpirationTime { get; set; }
    }
}
