using System.Threading.Tasks;
using InventoryService.Entities;
using MassTransit;
using Play.Common.Contracts;
using Play.Common.Repositories;

namespace InventoryService.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> catalogItemRepository;
        public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            this.catalogItemRepository = catalogItemRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            await catalogItemRepository.RemoveAsync(context.Message.Id);
        }
    }
}