using Microsoft.EntityFrameworkCore;
using SimpleBlogCore.Domain.Entities;
using SimpleBlogCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogCore.DataProvider.Repositories
{
    public class EfAsyncBaseRepository<T> : IAsyncRepository<T> where T : BaseEntity, new()
    {
        private readonly SimpleBlogDbContext context;

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

        public async Task Remove(Guid entityId)
        {
            context.Set<T>().Remove(new T { Id = entityId });
            await context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
