using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Contexts;
using TaskManager.Data.Entities.Abstracts;
using TaskManager.Repository.Helpers;
using TaskManager.Repository.Interfaces;

namespace TaskManager.Repository.Implementations
{
    public class Repository<T> : IReadCreateUpdateDeleteRepo<T>
        where T : Entity
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public async Task<PagedList<TSearchable>> GetAllAsync<TSearchable>(ItemQueryParameters queryParameters)
            where TSearchable : SearchableEntity, T
        {
            var query = _context.Set<TSearchable>().AsQueryable();
            var pageList = await GetAllAsyncPrivate(query, queryParameters);
            return pageList;
        }
        public async Task<PagedList<TSearchable>> GetAllAsync<TSearchable>(Expression<Func<TSearchable, bool>> criteria, ItemQueryParameters queryParameters)
            where TSearchable : SearchableEntity, T
        {
            var query = _context.Set<TSearchable>().Where(criteria).AsQueryable();
            var pageList = await GetAllAsyncPrivate(query, queryParameters);
            return pageList;
        }
        private async Task<PagedList<TSearchable>> GetAllAsyncPrivate<TSearchable>(IQueryable<TSearchable> query ,ItemQueryParameters queryParameters)
            where TSearchable : SearchableEntity, T
        {
            // Filteration
            if (!string.IsNullOrEmpty(queryParameters.Category))
            {
                query = query.Where(i => i.SearchableProperty.Contains(queryParameters.Category));
            }

            // Sorting
            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                bool descending = queryParameters.Sort.EndsWith("_desc");
                string sortField = queryParameters.Sort.Replace("_desc", "").Replace("_asc", "");

                // Apply Sorting
                // Apply Sorting dynamically
                query = ApplySorting(query, sortField, descending);
            }

            // Pagination
            var pageList = await PagedList<TSearchable>.CreateAsync(query, queryParameters.Page, queryParameters.Limit);

            return pageList;
        }
        private IQueryable<TSearchable> ApplySorting<TSearchable>(IQueryable<TSearchable> query, string sortField, bool descending)
            where TSearchable : SearchableEntity, T
        {
            var propertyInfo = typeof(T).GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo is null) return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortField);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = descending ? "OrderByDescending" : "OrderBy";

            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<TSearchable>)method.Invoke(null, [query, lambda]);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }
        public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }
    }
}
