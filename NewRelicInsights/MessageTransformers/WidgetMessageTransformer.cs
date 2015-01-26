using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.NewRelicInsights.Models.Messages;
using Orchard.Environment.Extensions;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers 
{
    [OrchardFeature("Glimpse.Orchard.NewRelicInsights")]
    public class WidgetMessageTransformer : NewRelicInsightsMessageTransformer<WidgetMessage>
    {
        public override string EventName
        {
            get { return "OrchardWidget"; }
        }

        protected override NewRelicInsightsMessage TransformMessage(WidgetMessage message) 
        {
            return new NewRelicWidgetMessage
            {
                WidgetName = message.Title,
                WidgetType = message.Type,
                Zone = message.Zone,
                Layer = message.Layer.Name,
                Duration = message.Duration.TotalMilliseconds,
            };
        }
    }
}