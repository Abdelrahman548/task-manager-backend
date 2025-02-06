using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Repository.Interfaces.Base
{
    public interface IUpdateRepository<T>
        where T : Entity
    {
        void Update(T item);
    }
}
