using System;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Glimpse.Orchard.Glimpse.Models;
using Glimpse.Orchard.MessageBrokers;
using Orchard.Core.Common.Utilities;

namespace Glimpse.Orchard.Glimpse.MessageBrokers {
    public class GlimpseMessageBroker : IPerformanceMessageBroker 
    {
        private readonly LazyField<IMessageBroker> _messageBroker;

        public GlimpseMessageBroker()
        {
            _messageBroker = new LazyField<IMessageBroker>();

            _messageBroker.Loader(() => {
                var context = HttpContext.Current;
                if (context == null)
                {
                    return new NullMessageBroker();
                }

                return ((GlimpseRuntime)context.Application.Get("__GlimpseRuntime")).Configuration.MessageBroker;
            });
        }

        public void Publish<T>(T message)
        {
            if (message is ITimelineMessage)
            {
                _messageBroker.Value.Publish(message);
                return;
            }

            var wrappedMessage = new GlimpseMessage<T>(message);
            _messageBroker.Value.Publish(wrappedMessage);
        }

        public class NullMessageBroker : IMessageBroker
        {
            public void Publish<T>(T message) { }

            public Guid Subscribe<T>(Action<T> action)
            {
                return Guid.NewGuid();
            }

            public void Unsubscribe<T>(Guid subscriptionId) { }
        }
    }
}