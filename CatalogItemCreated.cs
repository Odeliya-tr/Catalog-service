using System;

namespace Play.Common.Contracts
{
    public record CatalogItemCreated(Guid Id, string Name, string Description);
}