﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<DepartmentResponseDto>>> Get(Guid id)
        {
            var result = await departmentService.Get(id);

            return StatusCode((int)result.StatusCode, result);
        }
        
        [HttpPost("")]
        public async Task<ActionResult<BaseResult<DepartmentResponseDto>>> Add(DepartmentRequestDto dto)
        {
            var result = await departmentService.Add(dto);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("")]
        public async Task<ActionResult<BaseResult<PagedList<DepartmentResponseDto>>>> GetAll([FromQuery]ItemQueryParameters queryParameters)
        {
            var result = await departmentService.Get(queryParameters);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
