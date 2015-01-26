using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.NewRelicInsights.Models.Messages;
using Orchard.Environment.Extensions;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers
{
    [OrchardFeature("Glimpse.Orchard.NewRelicInsights")]
    public class CacheMessageTransformer : NewRelicInsightsMessageTransformer<CacheMessage>
    {
        public override string EventName
        {
            get { return "OrchardCache"; }
        }

        protected override NewRelicInsightsMessage TransformMessage(CacheMessage message) 
        {
            return new NewRelicCacheMessage
            {
                Action = message.Action,
                Key = message.Key,
                Result = message.Result,
                Duration = message.Duration.TotalMilliseconds,
            };
        }
    }
}