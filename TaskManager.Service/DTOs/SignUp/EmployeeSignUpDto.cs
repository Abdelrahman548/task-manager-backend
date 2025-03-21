﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.DTOs.Request;

namespace TaskManager.Service.DTOs.SignUp
{
    public class EmployeeSignUpDto: EmployeeRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MaxLength(50), MinLength(10)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@#$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-])[A-Za-z\\d@$!%*?&#^(){}[\\]<>_+=|\\\\~`:;,.\\/-]{10,}$")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MaxLength(10)]
        public string OTPEmailVerifyCode { get; set; } = string.Empty;
    }
}
