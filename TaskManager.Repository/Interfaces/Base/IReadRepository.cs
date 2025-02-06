using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Repository.Helpers;

namespace TaskManager.Repository.Interfaces.Base
{
    public interface IReadRepository<T>
        where T : Entity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<PagedList<T>> GetAllAsync(ItemQueryParameters queryParameters);
    }
}
