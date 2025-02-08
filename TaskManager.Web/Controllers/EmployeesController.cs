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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<EmployeeResponseDto>>> Get(Guid id)
        {
            var result = await employeeService.Get(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<EmployeeResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await employeeService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid id, EmployeeRequestDto dto)
        {
            var result = await employeeService.Update(dto, id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid id)
        {
            var result = await employeeService.Delete(id);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
