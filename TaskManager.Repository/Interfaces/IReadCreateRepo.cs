using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Repository.Interfaces.Base;

namespace TaskManager.Repository.Interfaces
{
    public interface IReadCreateRepo<T> : IReadRepository<T>, ICreateRepository<T>
        where T : Entity
    {
    }
}
