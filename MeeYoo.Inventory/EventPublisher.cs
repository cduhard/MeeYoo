namespace MeeYoo.Inventory
{
    public interface EventPublisher
    {
        void Publish<TEvent>(TEvent @event);
    }
}