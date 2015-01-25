using Glimpse.Orchard.NewRelicInsights.Models.Messages;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers 
{
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