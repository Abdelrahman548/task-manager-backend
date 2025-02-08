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
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService managerService;

        public ManagersController(IManagerService managerService)
        {
            this.managerService = managerService;
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<ManagerResponseDto>>> Get(Guid id)
        {
            var result = await managerService.Get(id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<ManagerResponseDto>>>> GetAll([FromQuery] ItemQueryParameters queryParameters)
        {
            var result = await managerService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<BaseResult<Guid>>> Update(Guid id, ManagerRequestDto dto)
        {
            var result = await managerService.Update(dto, id);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<BaseResult<string>>> Delete(Guid id)
        {
            var result = await managerService.Delete(id);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
