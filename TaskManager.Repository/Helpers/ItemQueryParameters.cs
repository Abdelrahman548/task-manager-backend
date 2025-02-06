using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Repository.Helpers
{
    public class ItemQueryParameters
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Category { get; set; }
        public string? Sort { get; set; }
    }
}
