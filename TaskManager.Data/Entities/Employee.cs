using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Entities
{
    public class Employee : Person
    {
        //Forign Keys
        public Guid DepartmentId { get; set; }


        // Navigational Properties
        public virtual Department? Department { get; set; }
        public virtual ICollection<MyTask> Tasks { get; set; } = [];
    }
}
