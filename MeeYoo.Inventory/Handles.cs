namespace MeeYoo.Inventory
{
    public interface Handles<in T> where T : class
    {
        void Handle(T message);
    }
}