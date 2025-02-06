using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Service.DTOs.Request;

namespace TaskManager.Service.DTOs.Response
{
    public class MyTaskResponseDto: MyTaskRequestDto
    {
        public Guid ID { get; set; } = Guid.Empty;
    }
}
