﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.DTOs.Request
{
    public class DepartmentRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
    }
}
