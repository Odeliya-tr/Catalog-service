using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogService.Entities;

namespace CatalogService.Repositories
{
    public interface IItemRepository
    {
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task CreateAsync(Item item);
        Task UpdateAsync(Item item);
        Task RemoveAsync(Guid id);
    }
}