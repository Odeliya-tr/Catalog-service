using System;

namespace InventoryService.Dtos
{
    public record InventoryItemDto
    {
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
        public InventoryItemDto(Guid catalogItemId, int quantity, DateTimeOffset acquiredDate)
        {
            CatalogItemId = catalogItemId;
            Quantity = quantity;
            AcquiredDate = acquiredDate;
        }
    }
}