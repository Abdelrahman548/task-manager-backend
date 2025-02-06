using AutoMapper;
using TaskManager.Repository.Helpers;
using TaskManager.Repository.Interfaces;
using TaskManager.Service.DTOs.Request;
using TaskManager.Service.DTOs.Response;
using TaskManager.Service.Helpers;
using TaskManager.Service.Interfaces;

namespace TaskManager.Service.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork repo;
        private readonly IMapper mapper;

        public EmployeeService(IUnitOfWork repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<BaseResult<string>> Delete(Guid employeeId)
        {
            var emp = await repo.Employees.GetByIdAsync(employeeId);
            if (emp is null) return new() { IsSuccess = false, Errors = ["Not Found"] , StatusCode = MyStatusCode.NotFound};
            repo.Employees.Delete(emp);
            return new() { IsSuccess = true, Message = Messages.DELETE_SUCCESS };
        }

        public async Task<BaseResult<EmployeeResponseDto>> Get(Guid employeeId)
        {
            var emp = await repo.Employees.GetByIdAsync(employeeId);
            if (emp is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            var employeeDto = mapper.Map<EmployeeResponseDto>(emp);
            return new() { IsSuccess = true, Message = Messages.GET_SUCCESS , Data = employeeDto, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<EmployeeResponseDto>>> Get(ItemQueryParameters criteria)
        {
            var pageList = await repo.Employees.GetAllAsync(criteria);
            var responsePageList = new PagedList<EmployeeResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<EmployeeResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<Guid>> Update(EmployeeRequestDto employeeDto, Guid employeeId)
        {
            var emp = await repo.Employees.GetByIdAsync(employeeId);
            if (emp is null) return new() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            mapper.Map(employeeDto, emp);
            await repo.CompeleteAsync();
            return new() { IsSuccess = false, Message = Messages.UPDATE_SUCCESS, Data = emp.ID, StatusCode = MyStatusCode.OK };
        }
    }
}
