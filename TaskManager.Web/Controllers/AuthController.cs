using Microsoft.AspNetCore.Mvc;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.DTOs.SignIn;
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
        [HttpPost("")]
        public async Task<ActionResult<BaseResult<LoginResponseDto>>> Login(LoginRequestDto dto)
        {
            var result = await authService.Login(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("Employee")]
        public async Task<ActionResult<BaseResult<string>>> SiginInEmployee(EmployeeSignInDto dto)
        {
            var result = await authService.SignIn(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("Manager")]
        public async Task<ActionResult<BaseResult<string>>> SiginInEmployee(ManagerSignInDto dto)
        {
            var result = await authService.SignIn(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
