using System.Threading.Tasks;
using InventoryService.Entities;
using MassTransit;
using Play.Common.Contracts;
using Play.Common.Repositories;

namespace InventoryService.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> catalogItemRepository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository)
        {
            this.catalogItemRepository = catalogItemRepository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await catalogItemRepository.GetAsync(message.Id);

            if (item == null)
            {
                return;
            }

            item.Name = message.Name;
            item.Description = message.Description;

            await catalogItemRepository.UpdateAsync(item);
        }
    }
}