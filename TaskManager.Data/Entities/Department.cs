using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Entities
{
    public class Department : Entity
    {
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        // Navigational Properties
        public virtual ICollection<Manager> Managers { get; set; } = [];
        public virtual ICollection<Employee> Employees { get; set; } = [];
        public virtual ICollection<MyTask> MyTasks { get; set; } = [];
    }
}
