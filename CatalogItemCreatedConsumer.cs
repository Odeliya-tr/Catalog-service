using System.Threading.Tasks;
using MassTransit;
using Play.Common.Contracts;
using Play.Common.Repositories;
using InventoryService.Entities;

namespace InventoryService.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> catalogItemRepository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            this.catalogItemRepository = catalogItemRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = new CatalogItem
            {
                Id = message.Id,
                Description = message.Description,
                Name = message.Name
            };

            await catalogItemRepository.CreateAsync(item);
        }
    }
}