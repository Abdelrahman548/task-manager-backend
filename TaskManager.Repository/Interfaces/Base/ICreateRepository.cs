using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;

namespace TaskManager.Repository.Interfaces.Base
{
    public interface ICreateRepository<T>
        where T : Entity
    {
        Task AddAsync(T item);
    }
}
