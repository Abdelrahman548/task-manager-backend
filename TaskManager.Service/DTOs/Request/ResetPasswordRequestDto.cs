using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.DTOs.Request
{
    public class ResetPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string OTPCode { get; set; } = string.Empty;
        [Required]
        [MaxLength(50), MinLength(10)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-])[A-Za-z\\d@$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-]{10,}$")]
        public string NewPassword { get; set; } = string.Empty;

    }
}
