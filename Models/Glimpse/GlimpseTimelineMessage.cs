using System;
using Glimpse.Core.Message;

namespace Glimpse.Orchard.Models.Glimpse {
    public class GlimpseTimelineMessage : MessageBase, ITimelineMessage
    {
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }
    }
}