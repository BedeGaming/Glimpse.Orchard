using System;
using Glimpse.Orchard.PerformanceMonitors.Models;
using Orchard;

namespace Glimpse.Orchard.Timers
{
    public interface IPerformanceTimer : IDependency {
        TimedActionResult<T> Time<T>(Func<T> action);
        TimerResult Time(Action action);
    }
}