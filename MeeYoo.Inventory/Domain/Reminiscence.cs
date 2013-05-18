using System;
using MeeYoo.Inventory.Messages;

namespace MeeYoo.Inventory.Domain
{
    public partial class Reminiscence : AggregateRoot
    {
        private bool counting;
        private Guid id;
        private int milstonesRecorded;
        private string name;

        protected Reminiscence()
        {
        }

        public Reminiscence(Guid reminiscenceId, Guid inventoryItemId, Guid warehouseId, string sku,
                             string description)
        {
            ApplyChange(new ItemTracked(reminiscenceId, inventoryItemId, warehouseId, sku, description));
        }

        public override Guid Id
        {
            get { return id; }
        }

        private void ShouldNotBeCounting()
        {
            Guard.Against(counting, "Cycle count for this item has begun.");
        }


        public void AdjustQuantity(int quantity)
        {
            ShouldNotBeCounting();
            ApplyChange(new ItemQuantityAdjusted(id, quantity));
        }

        public void Relocate(string location)
        {
            ShouldNotBeCounting();
            ApplyChange(new ItemRelocated(id, location));
        }

        public void StartCycleCount()
        {
            ApplyChange(new CycleCountStarted(id, milstonesRecorded));
        }

        public void CompleteCycleCount(int quantityFound)
        {
            ApplyChange(new CycleCountCompleted(id, quantityFound));
            ApplyChange(new ItemQuantityAdjusted(id, quantityFound - milstonesRecorded));
        }

        public void Pick(int quantity)
        {
            ShouldNotBeCounting();
            Guard.Against<ArgumentOutOfRangeException>(quantity == 0, "quantity", "Tried to pick 0 quantity.");
            Guard.Against<ArgumentOutOfRangeException>(quantity < 0, "quantity",
                                                       "You tried to pick a negative quantity. Did you mean to receive or make an adjustment?");
            ApplyChange(new ItemPicked(id, quantity));
        }

        public void Liquidate(int quantity)
        {
            ShouldNotBeCounting();
            Guard.Against<ArgumentOutOfRangeException>(quantity == 0, "quantity", "Tried to liquidate 0 quantity.");
            Guard.Against<ArgumentOutOfRangeException>(quantity < 0, "quantity",
                                                       "You tried to liquidate a negative quantity. Did you mean to receive or make an adjustment?");
            ApplyChange(new ItemLiquidated(id, quantity));
        }

        public void RecordMilestone(string milestoneMessage, DateTime occurredOn)
        {
            ApplyChange(new MilestoneRecorded(id, milestoneMessage, occurredOn));
        }
    }
}