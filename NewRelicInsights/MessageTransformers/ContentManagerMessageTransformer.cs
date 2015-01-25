using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.NewRelicInsights.Models.Messages;
using Orchard.Environment.Extensions;

namespace Glimpse.Orchard.NewRelicInsights.MessageTransformers
{
    [OrchardFeature("Glimpse.Orchard.NewRelicInsights")]
    public class ContentManagerMessageTransformer : NewRelicInsightsMessageTransformer<ContentManagerMessage>
    {
        public override string EventName
        {
            get { return "OrchardContentManager"; }
        }

        protected override NewRelicInsightsMessage TransformMessage(ContentManagerMessage message) 
        {
            return new NewRelicContentManagerMessage
            {
                ContentId = message.ContentId,
                ContentType = message.ContentType,
                Name = message.Name,
                Duration = message.Duration.TotalMilliseconds,
            };
        }
    }
}