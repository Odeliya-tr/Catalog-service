using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using InventoryService.Dtos;

namespace InventoryService.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CatalogItemDto> GetCatalogItemAsync(Guid id)
        {
            return await httpClient.GetFromJsonAsync<CatalogItemDto>($"items/{id}");
        }
    }
}