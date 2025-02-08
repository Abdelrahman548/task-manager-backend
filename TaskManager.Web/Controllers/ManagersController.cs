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
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService managerService;

        public ManagersController(IManagerService managerService)
        {
            this.managerService = managerService;
        }

        [HttpGet("Current")]
        public async Task<ActionResult<BaseResult<AdminResponseDto>>> Get()
        {
            var managerIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (managerIdStr is null)
            {
                return Unauthorized(
                        new BaseResult<Guid>() { IsSuccess = false, Errors = ["UnAuthenticated"], StatusCode = MyStatusCode.Unauthorized }
                    );
            }
            var managerID = new Guid(managerIdStr);
            var result = await managerService.Get(managerID);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{managerId:guid}")]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<ActionResult<BaseResult<ManagerResponseDto>>> Get(Guid managerId)
        {
            var result = await managerService.Get(managerId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<BaseResult<PagedList<ManagerResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await managerService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{managerId:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid managerId, ManagerRequestDto dto)
        {
            var result = await managerService.Update(dto, managerId);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{managerId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid managerId)
        {
            var result = await managerService.Delete(managerId);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
