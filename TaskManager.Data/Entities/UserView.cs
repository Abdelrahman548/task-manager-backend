using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Entities
{
    public class UserView: Entity
    {
        [EmailAddress]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;
    }
}
