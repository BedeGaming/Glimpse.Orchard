using System;

namespace Glimpse.Orchard.PerformanceMonitors.Models
{
    public interface ITimedPerfMonMessage
    {
        TimeSpan Duration { get; set; }
        TimeSpan Offset { get; set; }
        DateTime StartTime { get; set; }
    }
}