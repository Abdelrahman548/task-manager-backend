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
    public interface IEmployeeService
    {
        Task<BaseResult<EmployeeResponseDto>> Get(Guid employeeId);
        Task<BaseResult<PagedList<EmployeeResponseDto>>> Get(ItemQueryParameters criteria);
        Task<BaseResult<Guid>> Update(EmployeeRequestDto employee, Guid employeeId);
        Task<BaseResult<string>> Delete(Guid employeeId);
    }
}
