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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("{employeeId:guid}")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<EmployeeResponseDto>>> Get(Guid employeeId)
        {
            var result = await employeeService.Get(employeeId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<BaseResult<PagedList<EmployeeResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await employeeService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{employeeId:guid}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid employeeId, EmployeeRequestDto dto)
        {
            var result = await employeeService.Update(dto, employeeId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{employeeId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid employeeId)
        {
            var result = await employeeService.Delete(employeeId);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
