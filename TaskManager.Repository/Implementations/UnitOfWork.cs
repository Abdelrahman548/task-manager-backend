using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Contexts;
using TaskManager.Data.Entities;
using TaskManager.Repository.Interfaces;

namespace TaskManager.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Admins = new Repository<Admin>(_dbContext);
            Employees = new Repository<Employee>(_dbContext);
            Managers = new Repository<Manager>(_dbContext);
            Departments = new Repository<Department>(_dbContext);
            MyTasks = new Repository<MyTask>(_dbContext);
            OTPVerifications = new Repository<OTPVerification>(_dbContext);
            UsersView = new Repository<UserView>(_dbContext);
            RefreshTokens = new Repository<RefreshToken>(_dbContext);

        }
        public IReadCreateUpdateRepo<Admin> Admins { get; }

        public IReadCreateUpdateDeleteRepo<MyTask> MyTasks { get; }

        public IReadCreateUpdateDeleteRepo<Employee> Employees { get; }

        public IReadCreateUpdateDeleteRepo<Manager> Managers { get; }

        public IReadCreateUpdateDeleteRepo<Department> Departments { get; }
        public IReadCreateUpdateDeleteRepo<OTPVerification> OTPVerifications { get; }

        public IReadCreateUpdateDeleteRepo<UserView> UsersView { get; }

        public IReadCreateUpdateDeleteRepo<RefreshToken> RefreshTokens { get; }

        public async Task<int> CompeleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
