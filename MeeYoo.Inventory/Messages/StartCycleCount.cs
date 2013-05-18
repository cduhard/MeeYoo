using System;

namespace MeeYoo.Inventory.Messages
{
    public class StartCycleCount : Command
    {
        public readonly Guid WarehouseItemId;

        private StartCycleCount()
        {
        }

        public StartCycleCount(Guid warehouseItemId)
        {
            WarehouseItemId = warehouseItemId;
        }

        public override string ToString()
        {
            return "Starting cycle count.";
        }
    }
}