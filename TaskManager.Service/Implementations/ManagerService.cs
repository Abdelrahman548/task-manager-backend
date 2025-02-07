using AutoMapper;
using TaskManager.Data.Entities;
using TaskManager.Repository.Helpers;
using TaskManager.Repository.Interfaces;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class ManagerService : IManagerService
    {
        private readonly IUnitOfWork repo;
        private readonly IMapper mapper;

        public ManagerService(IUnitOfWork repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<BaseResult<string>> Delete(Guid managerId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            repo.Managers.Delete(manager);
            return new() { IsSuccess = true, Message = Messages.DELETE_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<ManagerResponseDto>> Get(Guid managerId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            var managerDto = mapper.Map<ManagerResponseDto>(manager);
            return new() { IsSuccess = true, Message = Messages.GET_SUCCESS, Data = managerDto, StatusCode = MyStatusCode.OK};
        }

        public async Task<BaseResult<PagedList<ManagerResponseDto>>> Get(ItemQueryParameters criteria)
        {
            var pageList = await repo.Managers.GetAllAsync(criteria);
            var responsePageList = new PagedList<ManagerResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<ManagerResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<Guid>> Update(ManagerRequestDto managerDto, Guid managerId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound};
            if (manager.DepartmentID != managerDto.DepartmentId)
            {
                var department = await repo.Departments.GetByIdAsync(managerDto.DepartmentId);
                if (department is null) return new() { IsSuccess = false, Errors = ["Department is Not Found"], StatusCode = MyStatusCode.NotFound };
            }
            mapper.Map(managerDto, manager);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS, Data = manager.ID, StatusCode = MyStatusCode.OK};
        }
    }
}
