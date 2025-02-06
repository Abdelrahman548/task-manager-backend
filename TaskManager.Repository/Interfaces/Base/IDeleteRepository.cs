using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Repository.Interfaces.Base
{
    public interface IDeleteRepository<T>
        where T : Entity
    {
        void Delete(T item);
    }
}
