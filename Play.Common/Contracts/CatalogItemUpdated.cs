using System;

namespace Play.Common.Contracts
{
    public record CatalogItemUpdated(Guid Id, string Name, string Description);
}