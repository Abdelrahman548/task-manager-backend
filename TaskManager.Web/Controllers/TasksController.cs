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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost("")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<BaseResult<MyTaskResponseDto>>> Add(MyTaskRequestDto dto)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var managerID = new Guid(id);
            dto.ManagerId = managerID;
            var result = await taskService.Add(dto,managerID,dto.EmployeeId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{taskId:guid}")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<MyTaskResponseDto>>> Get(Guid taskId)
        {
            var result = await taskService.Get(taskId);

            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpPut("{taskId:guid}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid taskId, MyTaskRequestDto dto)
        {
            var managerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerIdStr is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var managerID = new Guid(managerIdStr);
            var result = await taskService.Update(taskId, dto, managerID);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{taskId:guid}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid taskId)
        {
            var managerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerIdStr is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var managerID = new Guid(managerIdStr);
            var result = await taskService.Delete(taskId, managerID);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<PagedList<MyTaskResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await taskService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Employee/{employeeId:guid}")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<PagedList<MyTaskResponseDto>>>> GetAllEmployeeTasks(Guid employeeId, [FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await taskService.GetEmployeeTasks(employeeId, queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Manager/{managerId:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<BaseResult<PagedList<MyTaskResponseDto>>>> GetAllManagerTasks(Guid managerId, [FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await taskService.GetManagerTasks(managerId, queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{taskId:guid}/Employee/{employeeId:guid}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<BaseResult<MyTaskResponseDto>>> ReAssign(Guid taskId, Guid employeeId)
        {
            var managerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerIdStr is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var managerID = new Guid(managerIdStr);
            var result = await taskService.ReAssign(taskId,managerID, employeeId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("{taskId:guid}/Employee/")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<BaseResult<MyTaskResponseDto>>> UpdateStatus(Guid taskId, MyTaskRequestDto dto)
        {
            var employeeIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employeeIdStr is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var employeeId = new Guid(employeeIdStr);
            var result = await taskService.UpdateStatus(taskId, dto,employeeId);

            return StatusCode((int)result.StatusCode, result);
        }
    
    }
}
