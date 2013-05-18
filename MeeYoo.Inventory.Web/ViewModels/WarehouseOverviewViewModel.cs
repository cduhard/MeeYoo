using System;

namespace MeeYoo.Inventory.Web.ViewModels
{
    public class WarehouseOverviewViewModel
    {
        public WarehouseOverviewViewModel(Guid warehouseId, string name)
        {
            WarehouseId = warehouseId;
        }

        public Guid WarehouseId { get; private set; }
    }
}