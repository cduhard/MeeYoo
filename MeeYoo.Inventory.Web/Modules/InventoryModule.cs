using System;
using MeeYoo.Inventory.Web.Services;
using Nancy;

namespace MeeYoo.Inventory.Web.Modules
{
    public class InventoryModule : NancyModule
    {
        public InventoryModule(IItemDetailRepository items)
            : base("/inventory")
        {
            Get["/{reminiscenceId}"] = p =>
            {
                Guid warehouseItemId = p.warehouseItemId;

                return items.Get(warehouseItemId);
            };
        }
    }
}