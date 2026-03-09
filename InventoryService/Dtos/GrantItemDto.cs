using System;

namespace InventoryService.Dtos
{
    public record GrantItemDto
    {
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
    }
}