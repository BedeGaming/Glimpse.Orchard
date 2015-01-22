namespace Glimpse.Orchard.PerformanceMonitors.Models {
    public class TimedActionResult<T> {
        public TimerResult TimerResult { get; set; }
        public T ActionResult { get; set; }
    }
}