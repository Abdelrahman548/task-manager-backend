using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.DTOs.SignUp;
using TaskManager.Service.Helpers;
using TaskManager.Service.Implementations;
using TaskManager.Service.Interfaces;

namespace TaskManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResult<LoginResponseDto>>> Login(LoginRequestDto dto)
        {
            var result = await authService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("SignUp/Employee")]
        public async Task<ActionResult<BaseResult<string>>> SignInEmployee(EmployeeSignUpDto dto)
        {
            var result = await authService.SignIn(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("SignUp/Manager")]
        public async Task<ActionResult<BaseResult<string>>> SignInManager(ManagerSignUpDto dto)
        {
            var result = await authService.SignIn(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpPost("SignUp/Admin")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<string>>> SignInAdmin(AdminSignUpDto dto)
        {
            var result = await authService.SignIn(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
