using System;

namespace MeeYoo.Inventory.Messages
{
    public class MilestoneRecorded : Event
    {
        public readonly Guid ReminiscenceId;
        public readonly string MilestoneMessage;
        public readonly DateTime OccurredOn;

        public MilestoneRecorded(Guid reminiscenceId, string milestoneMessage, DateTime occurredOn)
        {
            ReminiscenceId = reminiscenceId;
            MilestoneMessage = milestoneMessage;
            OccurredOn = occurredOn;
        }

        public override string ToString()
        {
            return string.Format("Milestone {0}.", MilestoneMessage);
        }
    }
}