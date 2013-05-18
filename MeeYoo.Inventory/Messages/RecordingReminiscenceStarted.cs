using System;

namespace MeeYoo.Inventory.Messages
{
    public class RecordingReminiscenceStarted : Event
    {
        public readonly Guid ReminiscenceId;
        public readonly string Name;
        public readonly DateTime BirthDate;

        private RecordingReminiscenceStarted()
        {
        }

        public RecordingReminiscenceStarted(Guid reminiscenceId, string name, DateTime birthDate)
        {
            ReminiscenceId = reminiscenceId;
            Name = name;
            BirthDate = birthDate;
        }

        public override string ToString()
        {
            return string.Format(" Tracking {0} Started.", Name);
        }
    }
}