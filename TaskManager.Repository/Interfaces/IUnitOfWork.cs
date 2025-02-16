using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;

namespace TaskManager.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IReadCreateUpdateRepo<Admin> Admins { get; }
        IReadCreateUpdateDeleteRepo<MyTask> MyTasks { get; }
        IReadCreateUpdateDeleteRepo<Employee> Employees { get; }
        IReadCreateUpdateDeleteRepo<Manager> Managers { get; }
        IReadCreateUpdateDeleteRepo<Department> Departments { get; }
        IReadCreateUpdateDeleteRepo<OTPVerification> OTPVerifications { get; }
        IReadCreateUpdateDeleteRepo<UserView> UsersView { get; }
        IReadCreateUpdateDeleteRepo<RefreshToken> RefreshTokens { get; }
        Task<int> CompeleteAsync();
    }
}
