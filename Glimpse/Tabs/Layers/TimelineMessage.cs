using System;
using Glimpse.Core.Message;
using Glimpse.Orchard.PerformanceMonitors.Models;

namespace Glimpse.Orchard.Glimpse.Tabs.Layers {
    public class TimelineMessage : MessageBase, ITimelineMessage, ITimedPerfMonMessage
    {
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }
    }
}