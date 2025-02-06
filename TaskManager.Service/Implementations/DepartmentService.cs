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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork repo;
        private readonly IMapper mapper;

        public DepartmentService(IUnitOfWork repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<BaseResult<string>> Delete(Guid departmentId)
        {
            var departmet = await repo.Departments.GetByIdAsync(departmentId);
            if (departmet == null) return new BaseResult<string>() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            repo.Departments.Delete(departmet);
            await repo.CompeleteAsync();
            return new BaseResult<string>() { IsSuccess = true, Message = Messages.DELETE_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<DepartmentResponseDto>> Get(Guid departmentId)
        {
            var department = await repo.Departments.GetByIdAsync(departmentId);
            if (department == null) return new BaseResult<DepartmentResponseDto>() { IsSuccess = false, Errors = ["Not Found"], StatusCode = MyStatusCode.NotFound };
            var responseDto = mapper.Map<DepartmentResponseDto>(department);
            return new BaseResult<DepartmentResponseDto>() { IsSuccess = true, Data = responseDto , Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<DepartmentResponseDto>>> Get(ItemQueryParameters criteria)
        {
            var pageList = await repo.Departments.GetAllAsync(criteria);
            var responsePageList = new PagedList<DepartmentResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<DepartmentResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<Guid>> Add(DepartmentRequestDto departmentDto)
        {
            var department = mapper.Map<Department>(departmentDto);
            department.ID = Guid.NewGuid();
            await repo.Departments.AddAsync(department);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.ADD_SUCCESS, Data = department.ID , StatusCode = MyStatusCode.Created };
        }

        public async Task<BaseResult<Guid>> Update(DepartmentRequestDto departmentDto, Guid departmentId)
        {
            var department = await repo.Departments.GetByIdAsync(departmentId);
            if (department == null) return new() { IsSuccess = false, Errors= ["Not Found"], StatusCode = MyStatusCode.NotFound };
            mapper.Map(departmentDto, department);

            await repo.CompeleteAsync();

            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS, Data = department.ID, StatusCode = MyStatusCode.OK };
        }
    }
}
