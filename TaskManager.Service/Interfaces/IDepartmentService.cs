using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository.Helpers;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.Helpers;

namespace TaskManager.Service.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseResult<DepartmentResponseDto>> Get(Guid departmentId);
        Task<BaseResult<PagedList<DepartmentResponseDto>>> Get(ItemQueryParameters criteria);
        Task<BaseResult<PagedList<DepartmentResponseDto>>> GetEmployees(Guid departmentId, ItemQueryParameters criteria);
        Task<BaseResult<PagedList<DepartmentResponseDto>>> GetManagers(Guid departmentId, ItemQueryParameters criteria);
        Task<BaseResult<Guid>> Add(DepartmentRequestDto department);
        Task<BaseResult<Guid>> Update(DepartmentRequestDto department, Guid departmentId);
        Task<BaseResult<string>> Delete(Guid departmentId);
    }
}
