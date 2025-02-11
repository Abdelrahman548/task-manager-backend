using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data.Entities.Abstracts
{
    public abstract class SearchableEntity : Entity
    {
        public string SearchableProperty { get; set; } = string.Empty;
    }
}
