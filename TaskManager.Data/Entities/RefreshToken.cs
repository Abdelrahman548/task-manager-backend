using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Data.Entities
{
    public class RefreshToken : Entity
    {
        [MaxLength(128)]
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; } = string.Empty;
    }
}
