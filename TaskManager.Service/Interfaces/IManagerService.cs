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
    public interface IManagerService
    {
        Task<BaseResult<ManagerResponseDto>> Get(Guid managerId);
        Task<BaseResult<PagedList<ManagerResponseDto>>> Get(ItemQueryParameters criteria);
        Task<BaseResult<Guid>> Update(ManagerRequestDto manager, Guid managerId);
        Task<BaseResult<string>> Delete(Guid managerId);
    }
}
