using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeeYoo.Inventory.Infrastructure;

namespace MeeYoo.Inventory
{
    public abstract class AggregateRoot
    {
        private readonly List<Event> _changes = new List<Event>();

        public abstract Guid Id { get; }
        public int Version { get; private set; }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
            Version++;
        }

        public override string ToString()
        {
            return
                GetUncommittedChanges()
                    .Aggregate(new StringBuilder(), (current, e) => current.AppendLine(e.ToString()))
                    .ToString();
        }
    }
}