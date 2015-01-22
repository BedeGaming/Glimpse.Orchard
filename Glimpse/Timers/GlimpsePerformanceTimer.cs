using System;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.PerformanceMonitors.Models;
using Glimpse.Orchard.Timers;
using TimerResult = Glimpse.Orchard.PerformanceMonitors.Models.TimerResult;

namespace Glimpse.Orchard.Glimpse.Timers
{
    public class GlimpsePerformanceTimer : IPerformanceTimer
    {
        public TimedActionResult<T> Time<T>(Func<T> action)
        {
            var result = default(T);

            var executionTimer = GetTimer();

            if (executionTimer == null)
            {
                return new TimedActionResult<T> {
                    ActionResult = action(),
                    TimerResult = new TimerResult()
                };
            }

            var duration = executionTimer.Time(() => { result = action(); }).ToGenericTimerResult();

            return new TimedActionResult<T>
            {
                ActionResult = result,
                TimerResult = duration
            };
        }

        public TimerResult Time(Action action)
        {
            var executionTimer = GetTimer();

            if (executionTimer == null) 
            {
                action();
                return new TimerResult();
            }

            return executionTimer.Time(action).ToGenericTimerResult();
        }

        
        private IExecutionTimer GetTimer()
        {
            var context = HttpContext.Current;
            if (context == null)
            {
                return null;
            }

            return ((GlimpseRuntime)context.Application.Get("__GlimpseRuntime")).Configuration.TimerStrategy.Invoke();
        }
    }
}