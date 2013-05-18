using System.Linq;
using MeeYoo.Inventory.Application;
using MeeYoo.Inventory.Domain;
using MeeYoo.Inventory.Messages;
using MeeYoo.Inventory.Tests.Templates;
using Simple.Testing.ClientFramework;

namespace MeeYoo.Inventory.Tests.Specifications.Core
{
    public class TrackingSpecifications : WarehouseItemSpecifications
    {
        public Specification tracking_a_new_item()
        {
            return new CommandSpecification<Reminiscence, TrackItem>
            {
                AggregateId = AggregateId,
                OnHandler = repository => new InventoryHandlers(repository),
                When =
                    new TrackItem(AggregateId, InventoryItemId, WarehouseId, "12345", "Abraxo Cleaner"),
                Expect =
                {
                    result => result.Decisions
                                    .OfType<ItemTracked>(typeof (ItemTracked))
                                    .Count().Equals(1)
                }
            };
        }
    }
}