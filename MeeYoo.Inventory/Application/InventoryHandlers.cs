using MeeYoo.Inventory.Domain;
using MeeYoo.Inventory.Infrastructure;
using MeeYoo.Inventory.Messages;

namespace MeeYoo.Inventory.Application
{
    public class InventoryHandlers :
        Handles<RecordMilestone>
    {
        private readonly IRepository<Reminiscence> repository;

        public InventoryHandlers(IRepository<Reminiscence> repository)
        {
            this.repository = repository;
        }

        public void Handle(AdjustItemQuantity message)
        {
            var item = repository.GetById(message.WarehouseItemId);
            item.AdjustQuantity(message.Quantity);
            repository.Save(item, message.Id);
        }

        public void Handle(RecordMilestone message)
        {
            var item = repository.GetById(message.ReminiscenceId);
            item.RecordMilestone(message.MilestoneMessage, message.OccurredOn);
            repository.Save(item, message.Id);
        }

    }
}