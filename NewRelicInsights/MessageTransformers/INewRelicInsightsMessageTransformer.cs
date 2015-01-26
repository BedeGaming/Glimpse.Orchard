using Glimpse.Orchard.NewRelicInsights.Models;
using Glimpse.Orchard.NewRelicInsights.Models.Messages;
using Orchard;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers {
    public interface INewRelicInsightsMessageTransformer : IDependency
    {
        string EventName { get; }
        NewRelicInsightsMessage TransformMessage(object message);
    }
}