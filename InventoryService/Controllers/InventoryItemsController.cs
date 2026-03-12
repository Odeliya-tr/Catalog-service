using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryService.Dtos;
using InventoryService.Entities;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Repositories;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> inventoryItemRepository;

        public InventoryItemsController(IRepository<InventoryItem> inventoryItemRepository)
        {
            this.inventoryItemRepository = inventoryItemRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            var inventoryItems = (await inventoryItemRepository.GetAllAsync())
                .Where(item => item.UserId == userId)
                .Select(item => new InventoryItemDto(
                    item.CatalogItemId,
                    item.Quantity,
                    item.AcquiredDate
                ));

            return Ok(inventoryItems);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var inventoryItem = new InventoryItem
            {
                Id = Guid.NewGuid(),
                UserId = grantItemDto.UserId,
                CatalogItemId = grantItemDto.CatalogItemId,
                Quantity = grantItemDto.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow
            };

            await inventoryItemRepository.CreateAsync(inventoryItem);

            return Ok();
        }
    }
}