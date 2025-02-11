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
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork repo;
        private readonly IMapper mapper;

        public TaskService(IUnitOfWork repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public async Task<BaseResult<Guid>> Add(MyTaskRequestDto taskDto, Guid managerId, Guid employeeId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Manager is Not Found"], StatusCode = MyStatusCode.NotFound };
            var employee = await repo.Employees.GetByIdAsync(employeeId);
            if (employee is null) return new() { IsSuccess = false, Errors = ["Employee is Not Found"], StatusCode = MyStatusCode.NotFound };

            if(employee.DepartmentID != manager.DepartmentID) return new(){ IsSuccess = false, Errors = ["Employee is Not Found"], StatusCode = MyStatusCode.NotFound };
            
            var task = mapper.Map<MyTask>(taskDto);
            task.ID = Guid.NewGuid();

            await repo.MyTasks.AddAsync(task);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.ADD_SUCCESS, Data = task.ID, StatusCode = MyStatusCode.Created};
        }

        public async Task<BaseResult<string>> Delete(Guid taskId, Guid managerId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Manager is Not Found"], StatusCode = MyStatusCode.NotFound };

            var task = await repo.MyTasks.GetByIdAsync(taskId);
            if (task is null) return new() { IsSuccess = false, Errors = ["Task is Not Found"], StatusCode = MyStatusCode.NotFound };

            if(task.ManagerID != manager.ID) return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };
            repo.MyTasks.Delete(task);
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.DELETE_SUCCESS, StatusCode = MyStatusCode.OK};
        }

        public async Task<BaseResult<MyTaskResponseDto>> Get(Guid taskId)
        {
            var task = await repo.MyTasks.GetByIdAsync(taskId);
            if (task is null) return new() { IsSuccess = false, Errors = ["Task is Not Found"], StatusCode = MyStatusCode.NotFound };
            var resposeTask = mapper.Map<MyTaskResponseDto>(task);
            return new() { IsSuccess = true, Message = Messages.GET_SUCCESS, Data = resposeTask, StatusCode = MyStatusCode.OK};
        }

        public async Task<BaseResult<PagedList<MyTaskResponseDto>>> Get(ItemQueryParameters criteria)
        {
            var pageList = await repo.MyTasks.GetAllAsync<MyTask>(criteria);
            var responsePageList = new PagedList<MyTaskResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<MyTaskResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<Guid>> ReAssign(Guid taskId, Guid managerId, Guid employeeId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Manager is Not Found"], StatusCode = MyStatusCode.NotFound};
            var employee = await repo.Employees.GetByIdAsync(employeeId);
            if (employee is null) return new() { IsSuccess = false, Errors = ["Employee is Not Found"], StatusCode = MyStatusCode.NotFound};
            var task = await repo.MyTasks.GetByIdAsync(taskId);
            if (task is null) return new() { IsSuccess = false, Errors = ["Task is Not Found"], StatusCode = MyStatusCode.NotFound };

            if(task.ManagerID != manager.ID ) return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };
            if(task.DepartmentID != employee.DepartmentID) return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };

            task.EmployeeID = employee.ID;
            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.OP_SUCCESS , Data = task.ID, StatusCode = MyStatusCode.OK};
        }

        public async Task<BaseResult<Guid>> UpdateStatus(Guid taskId, MyTaskRequestDto taskDto, Guid employeeId)
        {
            var employee = await repo.Employees.GetByIdAsync(employeeId);
            if (employee is null) return new() { IsSuccess = false, Errors = ["Employee is Not Found"], StatusCode = MyStatusCode.NotFound };
            var task = await repo.MyTasks.GetByIdAsync(taskId);
            if (task is null) return new() { IsSuccess = false, Errors = ["Task is Not Found"], StatusCode = MyStatusCode.NotFound};

            if(task.EmployeeID != employee.ID) return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };
            
            // Update Task Status
            task.Status = taskDto.Status;

            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS,Data = task.ID ,StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<MyTaskResponseDto>>> GetEmployeeTasks(Guid employeeId, ItemQueryParameters queryParameters)
        {
            var employee = await repo.Employees.GetByIdAsync(employeeId);
            if (employee is null) return new() { IsSuccess = false, Errors = ["Employee is Not Found"], StatusCode = MyStatusCode.NotFound };

            var pageList = await repo.MyTasks.GetAllAsync<MyTask>(T => T.EmployeeID == employeeId, queryParameters);
            var responsePageList = new PagedList<MyTaskResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<MyTaskResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<PagedList<MyTaskResponseDto>>> GetManagerTasks(Guid managerId, ItemQueryParameters queryParameters)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Manager is Not Found"], StatusCode = MyStatusCode.NotFound };
            var pageList = await repo.MyTasks.GetAllAsync<MyTask>(T => T.ManagerID == managerId, queryParameters);
            var responsePageList = new PagedList<MyTaskResponseDto>(
                                    pageList.Items.Select(item => mapper.Map<MyTaskResponseDto>(item)).ToList(),
                                    pageList.Page,
                                    pageList.PageSize,
                                    pageList.TotalCount
                                );

            return new() { IsSuccess = true, Data = responsePageList, Message = Messages.GET_SUCCESS, StatusCode = MyStatusCode.OK };
        }

        public async Task<BaseResult<Guid>> Update(Guid taskId, MyTaskRequestDto taskDto, Guid managerId)
        {
            var manager = await repo.Managers.GetByIdAsync(managerId);
            if (manager is null) return new() { IsSuccess = false, Errors = ["Manager is Not Found"], StatusCode = MyStatusCode.NotFound };
            var task = await repo.MyTasks.GetByIdAsync(taskId);
            if (task is null) return new() { IsSuccess = false, Errors = ["Task is Not Found"], StatusCode = MyStatusCode.NotFound };

            if (task.ManagerID != manager.ID) return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };

            var employee = await repo.Managers.GetByIdAsync(taskDto.EmployeeId);
            if (employee is null) return new() { IsSuccess = false, Errors = ["Employee is Not Found"], StatusCode = MyStatusCode.NotFound };

            if(manager.DepartmentID != employee.DepartmentID) return new() { IsSuccess = false, Errors = ["Forbidden"], StatusCode = MyStatusCode.Forbidden };

            // Update Task Status
            mapper.Map(taskDto, task);

            await repo.CompeleteAsync();
            return new() { IsSuccess = true, Message = Messages.UPDATE_SUCCESS, Data = task.ID, StatusCode = MyStatusCode.OK };
        }
    }
}
