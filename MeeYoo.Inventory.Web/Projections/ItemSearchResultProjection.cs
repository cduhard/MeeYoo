using System;
using EventStore.ClientAPI;
using MeeYoo.Inventory.Messages;
using MeeYoo.Inventory.Web.ViewModels;
using MeeYoo.Inventory.Web.ViewWriters;

namespace MeeYoo.Inventory.Web.Projections
{
    public class ItemSearchResultProjection : ProjectionHandles<ItemTracked>
    {
        private readonly IWriteViews<Guid, ItemSearchResultViewModel> writer;

        public ItemSearchResultProjection(IWriteViews<Guid, ItemSearchResultViewModel> writer)
        {
            this.writer = writer;
        }

        #region ProjectionHandles<ItemTracked> Members

        public void Handle(ItemTracked message, Position? position)
        {
            writer.AddOrUpdate(
                message.WarehouseItemId,
                position,
                () => new ItemSearchResultViewModel(message.WarehouseItemId, message.WarehouseId)
                {
                    Description = message.Description,
                    Sku = message.Sku
                });
        }

        #endregion
    }
}