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
        Task<PagedList<TSearchable>> GetAllAsync<TSearchable>(ItemQueryParameters queryParameters)
            where TSearchable : SearchableEntity, T;
        Task<PagedList<TSearchable>> GetAllAsync<TSearchable>(Expression<Func<TSearchable, bool>> criteria, ItemQueryParameters queryParameters)
            where TSearchable : SearchableEntity, T;
        Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
    }
}
