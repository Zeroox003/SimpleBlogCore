using Microsoft.EntityFrameworkCore;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlogCore.DataProvider.Repositories
{
    public class EfAsyncBaseRepository<T> : IAsyncRepository<T> where T : BaseEntity, new()
    {
        protected readonly SimpleBlogDbContext context;

        protected IQueryable<T> QueryAll => context.Set<T>();

        public EfAsyncBaseRepository(SimpleBlogDbContext context)
        {
            this.context = context;
        }

        public async Task<T> Add(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid entityId)
        {
            return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == entityId);
        }

        public async Task RemoveById(Guid entityId)
        {
            context.Set<T>().Remove(new T { Id = entityId });
            await context.SaveChangesAsync();
        }

        public async Task Remove(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
