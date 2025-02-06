using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.DTOs.Request;

namespace TaskManager.Service.DTOs.SignIn
{
    public class AdminSignInDto : AdminRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MaxLength(50), MinLength(10)]
        public string Password { get; set; } = string.Empty;
    }
}
