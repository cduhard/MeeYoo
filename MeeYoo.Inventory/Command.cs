using System;

namespace MeeYoo.Inventory
{
    public class Command : Message
    {
        public readonly Guid Id = Guid.NewGuid();
    }
}