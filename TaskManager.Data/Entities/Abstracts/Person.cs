using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Helpers;

namespace TaskManager.Data.Entities.Abstracts
{
    public abstract class Person : Entity
    {
        [EmailAddress]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;
        [MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(20)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
