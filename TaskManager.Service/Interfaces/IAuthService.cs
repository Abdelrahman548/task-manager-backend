using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.DTOs.SignUp;
using TaskManager.Service.Helpers;

namespace TaskManager.Service.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResult<LoginResponseDto>> Login(LoginRequestDto user);
        Task<BaseResult<string>> SignIn(EmployeeSignUpDto employee);
        Task<BaseResult<string>> SignIn(ManagerSignUpDto manager);
        Task<BaseResult<string>> SignIn(AdminSignUpDto adminDto);
    }
}
