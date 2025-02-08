using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
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

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<AdminResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await adminService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid id, AdminRequestDto dto)
        {
            var result = await adminService.Update(id, dto);

            return StatusCode((int)result.StatusCode, result);
        }

    }
}
