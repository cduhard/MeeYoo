using MeeYoo.Inventory.Messages;

namespace MeeYoo.Inventory.Domain
{
    partial class Reminiscence
    {
        // ReSharper disable UnusedMember.Local
        private void Apply(ItemTracked e)
        {
            id = e.WarehouseItemId;
        }

        private void Apply(ItemQuantityAdjusted e)
        {
            milstonesRecorded += e.AdjustmentQuantity;
        }

        private void Apply(ItemPicked e)
        {
            milstonesRecorded -= e.Quantity;
        }

        private void Apply(ItemLiquidated e)
        {
            milstonesRecorded -= e.Quantity;
        }

        private void Apply(MilestoneRecorded e)
        {
            milstonesRecorded++;
        }


        private void Apply(RecordingReminiscenceStarted e)
        {
            name = e.Name;
        }

        private void Apply(CycleCountStarted e)
        {
            counting = true;
        }


    }
}