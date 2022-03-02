﻿using System;

namespace Play.Catalog.Constracts
{
    public record CatalogItemCreated(Guid ItemId, string Name, string Description);
    public record CatalogItemUpdate(Guid ItemId, string Name, string Description);

    public record CatalogItemDeleted(Guid ItemId);
}
