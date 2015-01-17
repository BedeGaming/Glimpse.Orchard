﻿using System;

namespace Glimpse.Orchard.PerformanceMonitors.Models {
    public class TimerResult
    {
        public TimeSpan Duration { get; set; }
        public TimeSpan Offset { get; set; }
        public DateTime StartTime { get; set; }
    }
}