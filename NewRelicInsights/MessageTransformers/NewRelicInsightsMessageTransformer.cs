using Glimpse.Orchard.NewRelicInsights.Models.Messages;
using Orchard.Environment.Extensions;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers
{
    [OrchardFeature("Glimpse.Orchard.NewRelicInsights")]
    public abstract class NewRelicInsightsMessageTransformer<T> : INewRelicInsightsMessageTransformer where T: class 
    {
        public NewRelicInsightsMessage TransformMessage(object message)
        {
            var messageToTransform = message as T;

            if (messageToTransform == null) {
                return null;
            }

            var transformedMessage = TransformMessage(messageToTransform);

            return transformedMessage;
        }

        public abstract string EventName { get; }
        protected abstract NewRelicInsightsMessage TransformMessage(T message);
    }
}