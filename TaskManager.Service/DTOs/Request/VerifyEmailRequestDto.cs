﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.DTOs.Request
{
    public class VerifyEmailRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
    }
}
