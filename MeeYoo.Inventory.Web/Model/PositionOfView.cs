﻿using EventStore.ClientAPI;

namespace MeeYoo.Inventory.Web.Model
{
    public class PositionOfView
    {
        public string Tag { get; set; }
        public long CommitPosition { get; set; }
        public long PreparePosition { get; set; }
        public string Id { get; set; }

        public static implicit operator Position?(PositionOfView document)
        {
            return document != null
                       ? new Position(document.CommitPosition, document.PreparePosition)
                       : default(Position?);
        }
    }
}