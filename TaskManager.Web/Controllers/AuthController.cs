using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        [HttpPost("Refresh")]
        public async Task<ActionResult<BaseResult<LoginResponseDto>>> Refresh(RefreshRequestDto dto)
        {
            var result = await authService.Refresh(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [Authorize(Roles = "Admin, Employee, Manager")]
        [HttpPost("Logout")]
        public async Task<ActionResult<BaseResult<string>>> Logout()
        {
            var IdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdStr is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var result = await authService.Logout(new(IdStr));
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("SignUp/Employee")]
        public async Task<ActionResult<BaseResult<string>>> SignInEmployee(EmployeeSignUpDto dto)
        {
            var result = await authService.SignUp(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("SignUp/Manager")]
        public async Task<ActionResult<BaseResult<string>>> SignInManager(ManagerSignUpDto dto)
        {
            var result = await authService.SignUp(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpPost("SignUp/Admin")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<string>>> SignInAdmin(AdminSignUpDto dto)
        {
            var result = await authService.SignUp(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("Email/Verify")]
        public async Task<ActionResult<BaseResult<string>>> VerifyEmail(VerifyEmailRequestDto verifyDto)
        {
            var result = await authService.VerifyEmail(verifyDto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("Password/Forgot")]
        public async Task<ActionResult<BaseResult<string>>> ForgotPassword(VerifyEmailRequestDto verifyDto)
        {
            var result = await authService.ForgotPassword(verifyDto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost("Password/Reset")]
        public async Task<ActionResult<BaseResult<string>>> ResetPassword(ResetPasswordRequestDto dto)
        {
            var result = await authService.ResetPassword(dto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
