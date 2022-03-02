using System.Threading.Tasks;
using MassTransit;
using play.Inventory.Service.Entities;
using Play.Catalog.Constracts;
using Play.Common;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await repository.GetAysnc(message.ItemId);
            if (item == null)
                return;
            await repository.RemoveAysnc(message.ItemId);
        }
    }
}