using EventStore.ClientAPI;

namespace MeeYoo.Inventory.Web.Projections
{
    public interface ProjectionHandles<in T>
    {
        void Handle(T message, Position? position);
    }
}