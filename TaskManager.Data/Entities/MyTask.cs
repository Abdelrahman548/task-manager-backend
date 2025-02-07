using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Data.Helpers;

namespace TaskManager.Data.Entities
{
    public class MyTask : Entity
    {
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public MyTaskPriority Priority { get; set; }
        public MyTaskStatus Status { get; set; }

        //Forign Keys
        public Guid DepartmentID {  get; set; }
        public Guid ManagerID {  get; set; }
        public Guid EmployeeID {  get; set; }

        // Navigational Properties
        public virtual Manager? Manager { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Department? Department { get; set; }
    }
}
