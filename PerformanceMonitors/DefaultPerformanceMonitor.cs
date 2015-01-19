using System;
using System.Collections.Generic;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.Glimpse.Tabs.Layers;
using Glimpse.Orchard.MessageBrokers;
using Glimpse.Orchard.Models;
using Glimpse.Orchard.PerformanceMonitors.Models;
using Glimpse.Orchard.Timers;
using Orchard;
using Orchard.Localization;
using Orchard.Logging;

namespace Glimpse.Orchard.PerformanceMonitors
{
    public class DefaultPerformanceMonitor : IPerformanceMonitor
    {
        private readonly IEnumerable<IPerformanceMessageBroker> _messageBrokers;
        private readonly IPerformanceTimer _performanceTimer;

        public DefaultPerformanceMonitor(IEnumerable<IPerformanceMessageBroker> messageBrokers, IPerformanceTimer performanceTimer)
        {
            _messageBrokers = messageBrokers;
            _performanceTimer = performanceTimer;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; private set; }

        public TimerResult Time(Action action) 
        {
            return _performanceTimer.Time(action);
        }

        public TimedActionResult<T> Time<T>(Func<T> action) 
        {
            return _performanceTimer.Time(action);
        }

        public TimerResult PublishTimedAction(Action action, PerfmonCategory category, string eventName, string eventSubText = null)
        {
            var timedResult = Time(action);
            PublishMessage(new TimelineMessage { EventName = eventName, EventCategory = category.ToGlimpseTimelineCategoryItem(), EventSubText = eventSubText }.AsTimedMessage(timedResult));

            return timedResult;
        }

        public TimerResult PublishTimedAction<T>(Action action, Func<T> messageFactory, PerfmonCategory category, string eventName, string eventSubText = null)
        {
            var timedResult = PublishTimedAction(action, category, eventName, eventSubText);
            PublishMessage(messageFactory());

            return timedResult;
        }

        public TimedActionResult<T> PublishTimedAction<T>(Func<T> action, PerfmonCategory category, string eventName, string eventSubText = null)
        {
            var timedResult = Time(action);
            PublishMessage(new TimelineMessage { EventName = eventName, EventCategory = category.ToGlimpseTimelineCategoryItem(), EventSubText = eventSubText }.AsTimedMessage(timedResult.TimerResult));

            return timedResult;
        }

        public TimedActionResult<T> PublishTimedAction<T>(Func<T> action, PerfmonCategory category, Func<T, string> eventNameFactory, Func<T, string> eventSubTextFactory = null)
        {
            var timedResult = Time(action);

            string eventSubText = null;
            if (eventSubTextFactory != null)
            {
                eventSubText = eventSubTextFactory(timedResult.ActionResult);
            }

            PublishMessage(new TimelineMessage { EventName = eventNameFactory(timedResult.ActionResult), EventCategory = category.ToGlimpseTimelineCategoryItem(), EventSubText = eventSubText }.AsTimedMessage(timedResult.TimerResult));

            return timedResult;
        }

        public TimedActionResult<T> PublishTimedAction<T, TMessage>(Func<T> action, Func<T, TimerResult, TMessage> messageFactory, PerfmonCategory category, string eventName, string eventSubText = null)
        {
            var actionResult = PublishTimedAction(action, category, eventName, eventSubText);
            PublishMessage(messageFactory(actionResult.ActionResult, actionResult.TimerResult));

            return actionResult;
        }

        public TimedActionResult<T> PublishTimedAction<T, TMessage>(Func<T> action, Func<T, TimerResult, TMessage> messageFactory, PerfmonCategory category, Func<T, string> eventNameFactory, Func<T, string> eventSubTextFactory = null)
        {
            var actionResult = PublishTimedAction(action, category, eventNameFactory, eventSubTextFactory);
            PublishMessage(messageFactory(actionResult.ActionResult, actionResult.TimerResult));

            return actionResult;
        }

        public void PublishMessage<T>(T message)
        {
            _messageBrokers.Invoke(broker =>broker.Publish(message), Logger);
        }
    }
}