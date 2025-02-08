using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Repository.Helpers;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.Helpers;
using TaskManager.Service.Implementations;
using TaskManager.Service.Interfaces;

namespace TaskManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<AdminResponseDto>>> Get(Guid id)
        {
            var result = await adminService.Get(id);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("Current")]
        public async Task<ActionResult<BaseResult<AdminResponseDto>>> Get()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var adminID = new Guid(id);
            var result = await adminService.Get(adminID);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<AdminResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await adminService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(AdminRequestDto dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var adminID = new Guid(id);
            var result = await adminService.Update(adminID, dto);

            return StatusCode((int)result.StatusCode, result);
        }

    }
}
