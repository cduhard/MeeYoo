using System;

namespace MeeYoo.Inventory.Messages
{
    public class RecordMilestone : Command
    {
        public readonly Guid ReminiscenceId;
        public readonly string MilestoneMessage;
        public readonly DateTime OccurredOn;

        private RecordMilestone()
        {
        }

        public RecordMilestone(Guid reminiscenceId, string milestoneMessage, DateTime occurredOn)
        {
            ReminiscenceId = reminiscenceId;
            MilestoneMessage = milestoneMessage;
            OccurredOn = occurredOn;
        }

        public override string ToString()
        {
            return string.Format("Recording {0}.", MilestoneMessage);
        }
    }
}