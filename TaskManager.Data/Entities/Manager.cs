﻿using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Entities
{
    public class Manager : Person
    {
        //Forign Keys
        public Guid DepartmentID { get; set; }


        // Navigational Properties
        public virtual Department? Department { get; set; }
        public virtual ICollection<MyTask> MyTasks { get; set; } = [];
    }
}
