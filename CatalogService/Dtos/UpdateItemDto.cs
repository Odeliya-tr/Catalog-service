using System.ComponentModel.DataAnnotations;

namespace CatalogService.Dtos
{
    public record UpdateItemDto(string Name, string Description, decimal Price);
}