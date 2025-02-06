using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Helpers;

namespace TaskManager.Service.DTOs.Request
{
    public class ManagerRequestDto
    {

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
