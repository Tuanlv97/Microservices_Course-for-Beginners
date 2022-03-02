using System.Threading.Tasks;
using MassTransit;
using play.Inventory.Service.Entities;
using Play.Catalog.Constracts;
using Play.Common;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdate>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdate> context)
        {
            var message = context.Message;
            var item = await repository.GetAysnc(message.ItemId);
            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description
                };
                await repository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                await repository.UpdateAsync(item);
            }

        }
    }
}