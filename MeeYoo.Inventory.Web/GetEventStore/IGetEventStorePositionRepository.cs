using EventStore.ClientAPI;

namespace MeeYoo.Inventory.Web.GetEventStore
{
    public interface IGetEventStorePositionRepository
    {
        Position? GetLastProcessedPosition();
    }
}