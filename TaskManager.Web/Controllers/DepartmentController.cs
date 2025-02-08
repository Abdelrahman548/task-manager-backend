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

        [HttpGet("{DepartmentId:guid}")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<DepartmentResponseDto>>> Get(Guid DepartmentId)
        {
            var result = await departmentService.Get(DepartmentId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<DepartmentResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await departmentService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{DepartmentId:guid}/Employees")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<PagedList<EmployeeResponseDto>>>> GetAllEmployees(Guid DepartmentId, [FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await departmentService.GetEmployees(DepartmentId,queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpGet("{DepartmentId:guid}/Managers")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<PagedList<ManagerResponseDto>>>> GetAllManagers(Guid DepartmentId, [FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await departmentService.GetManagers(DepartmentId,queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{DepartmentId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid DepartmentId, DepartmentRequestDto dto)
        {
            var result = await departmentService.Update(dto,DepartmentId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{DepartmentId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid DepartmentId)
        {
            var result = await departmentService.Delete(DepartmentId);

            return StatusCode((int)result.StatusCode, result);
        }

    }
}
