using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
    }
}
