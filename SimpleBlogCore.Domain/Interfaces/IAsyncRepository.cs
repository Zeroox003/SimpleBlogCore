using SimpleBlogCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlogCore.Domain.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> Add(T entity);

        Task RemoveById(Guid entityId);

        Task Remove(T entity);

        Task Update(T entity);

        Task<IReadOnlyList<T>> GetAll();

        Task<T> GetById(Guid entityId);
    }
}
