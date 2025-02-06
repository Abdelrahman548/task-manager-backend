using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.DTOs.Request;

namespace TaskManager.Service.DTOs.Response
{
    public class AdminResponseDto: AdminRequestDto
    {
        public Guid ID { get; set; } = Guid.Empty;
    }
}
