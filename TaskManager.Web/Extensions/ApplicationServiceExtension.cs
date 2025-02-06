using TaskManager.Repository.Implementations;
using TaskManager.Repository.Interfaces;
using TaskManager.Service.Implementations;
using TaskManager.Service.Interfaces;
using TaskManager.Service.Profiles;

namespace TaskManager.Web.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void AddApplicationServiceExtension(this IServiceCollection services)
        {
            // Repo Registration
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Auto Mapping Registration
            services.AddAutoMapper(typeof(EmployeeProfile));
            services.AddAutoMapper(typeof(ManagerProfile));
            services.AddAutoMapper(typeof(DepartmentProfile));
            services.AddAutoMapper(typeof(AdminProfile));
            services.AddAutoMapper(typeof(MyTaskProfile));

            // Services Registration
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IAuthService, AuthService>();


        }
    }
}
