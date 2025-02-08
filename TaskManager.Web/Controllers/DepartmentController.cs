using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Repository.Helpers;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }
        
        [HttpPost("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<DepartmentResponseDto>>> Add(DepartmentRequestDto dto)
        {
            var result = await departmentService.Add(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<DepartmentResponseDto>>> Get(Guid id)
        {
            var result = await departmentService.Get(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<DepartmentResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await departmentService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid id, DepartmentRequestDto dto)
        {
            var result = await departmentService.Update(dto,id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid id)
        {
            var result = await departmentService.Delete(id);

            return StatusCode((int)result.StatusCode, result);
        }

    }
}
