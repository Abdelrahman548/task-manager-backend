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
    public interface IAdminService
    {
        Task<BaseResult<AdminResponseDto>> Get(Guid adminId);
        Task<BaseResult<PagedList<AdminResponseDto>>> Get(ItemQueryParameters criteria);
        Task<BaseResult<Guid>> Update(Guid adminId, AdminRequestDto admin);
    }
}
