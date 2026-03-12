using System.ComponentModel.DataAnnotations;

namespace CatalogService.Dtos
{
    public record CreateItemDto(
        string Name,
        string Description,
        decimal Price
    );
}