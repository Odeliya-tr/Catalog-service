using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Common.Entities;

namespace Play.Common.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> GetAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(Guid id);
    }
}