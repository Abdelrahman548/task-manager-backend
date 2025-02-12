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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manager>().ToTable("Managers").Property(p => p.SearchableProperty).HasComputedColumnSql("FirstName + LastName");
            modelBuilder.Entity<Employee>().ToTable("Employees").Property(p => p.SearchableProperty).HasComputedColumnSql("FirstName + LastName");
            modelBuilder.Entity<Department>().ToTable("Departments").Property(p => p.SearchableProperty).HasComputedColumnSql("Title");
            modelBuilder.Entity<Admin>().ToTable("Admins").Property(p => p.SearchableProperty).HasComputedColumnSql("FirstName + LastName");
            modelBuilder.Entity<MyTask>().ToTable("Tasks").Property(p => p.SearchableProperty).HasComputedColumnSql("Title");

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

            modelBuilder.Entity<OTPVerification>()
                        .HasIndex(o => o.Email)
                        .IsUnique();

        }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<MyTask> MyTasks { get; set; }

    }
}
