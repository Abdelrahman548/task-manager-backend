using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Helpers;

namespace TaskManager.Service.DTOs.Request
{
    public class MyTaskRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public MyTaskPriority Priority { get; set; }
        [Required]
        public MyTaskStatus Status { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public Guid ManagerId { get; set; }
    }
}
