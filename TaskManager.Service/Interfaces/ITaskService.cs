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
    public interface ITaskService
    {
        Task<BaseResult<MyTaskResponseDto>> Get(Guid taskId);
        Task<BaseResult<PagedList<MyTaskResponseDto>>> GetEmployeeTasks(Guid emploeeId, ItemQueryParameters queryParameters);
        Task<BaseResult<PagedList<MyTaskResponseDto>>> GetManagerTasks(Guid managerId, ItemQueryParameters queryParameters);
        Task<BaseResult<PagedList<MyTaskResponseDto>>> Get(ItemQueryParameters criteria);
        Task<BaseResult<Guid>> Add(MyTaskRequestDto task, Guid managerId, Guid employeeId);
        Task<BaseResult<Guid>> ReAssign(Guid taskId, Guid managerId, Guid employeeId);
        Task<BaseResult<Guid>> UpdateStatus(Guid taskId, MyTaskRequestDto task, Guid employeeId);
        Task<BaseResult<Guid>> Update(Guid taskId, MyTaskRequestDto task, Guid managerId);
        Task<BaseResult<string>> Delete(Guid taskId, Guid managerId);
    }
}
