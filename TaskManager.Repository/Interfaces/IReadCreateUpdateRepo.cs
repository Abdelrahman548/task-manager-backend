using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Repository.Interfaces.Base;

namespace TaskManager.Repository.Interfaces
{
    public interface IReadCreateUpdateRepo<T> : IReadRepository<T>, ICreateRepository<T>, IUpdateRepository<T>
        where T : Entity
    {
    }
}
