using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.Helpers;

namespace TaskManager.Service.DTOs.Response
{
    public class LoginResponseDto
    {
        public Token? Token { get; set; }
    }
}
