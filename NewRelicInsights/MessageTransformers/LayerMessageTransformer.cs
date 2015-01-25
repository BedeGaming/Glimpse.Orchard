using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.NewRelicInsights.Models.Messages;
using Orchard.Environment.Extensions;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers
{
    [OrchardFeature("Glimpse.Orchard.NewRelicInsights")]
    public class LayerMessageTransformer : NewRelicInsightsMessageTransformer<LayerMessage>
    {
        public override string EventName
        {
            get { return "OrchardLayer"; }
        }

        protected override NewRelicInsightsMessage TransformMessage(LayerMessage message) 
        {
            return new NewRelicLayerMessage
            {
                LayerName = message.Name,
                LayerRule = message.Rule,
                Active = message.Active ? 1: 0,
                Duration = message.Duration.TotalMilliseconds,
            };
        }
    }
}