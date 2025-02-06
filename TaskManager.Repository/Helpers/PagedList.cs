using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Repository.Helpers
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;

        private PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public async static Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = query.Count();
            var items = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new(items, page, pageSize, totalCount);
        }
    }
}
