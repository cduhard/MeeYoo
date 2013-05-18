using System;

namespace MeeYoo.Inventory.Messages
{
    public class StartRecordingReminiscence : Command
    {
        public readonly Guid ReminiscenceId;
        public readonly string Name;
        public readonly DateTime BirthDate;

        private StartRecordingReminiscence()
        {
        }

        public StartRecordingReminiscence(Guid reminiscenceId, string name, DateTime birthDate)
        {
            ReminiscenceId = reminiscenceId;
            Name = name;
            BirthDate = birthDate;
        }

        public override string ToString()
        {
            return string.Format("Start Tracking {0}.", Name);
        }
    }
}