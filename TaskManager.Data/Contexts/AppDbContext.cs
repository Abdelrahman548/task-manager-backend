using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Contexts
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options): base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().ToTable("Managers").Ignore(e => e.SearchableProperty);
            modelBuilder.Entity<Employee>().ToTable("Employees").Ignore(e => e.SearchableProperty);
            modelBuilder.Entity<Department>().ToTable("Departments").Ignore(e => e.SearchableProperty);
            modelBuilder.Entity<Admin>().ToTable("Admins").Ignore(e => e.SearchableProperty);
            modelBuilder.Entity<MyTask>().ToTable("Tasks").Ignore(e => e.SearchableProperty);

            modelBuilder.Entity<MyTask>()
                        .HasOne(t => t.Employee)
                        .WithMany(t => t.MyTasks)
                        .HasForeignKey(t => t.EmployeeID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MyTask>()
                        .HasOne(t => t.Manager)
                        .WithMany(t => t.MyTasks)
                        .HasForeignKey(t => t.ManagerID)
                        .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<MyTask> MyTasks { get; set; }

    }
}
