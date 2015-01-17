using System;
using Orchard;

namespace Glimpse.Orchard.MessageBrokers
{
    public interface IPerformanceMessageBroker : IDependency
    {
        void Publish<T>(T message);
        Guid Subscribe<T>(Action<T> action);
        void Unsubscribe<T>(Guid subscriptionId);
    }
}