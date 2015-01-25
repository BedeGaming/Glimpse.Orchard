namespace Glimpse.Orchard.NewRelicInsights.Models.Messages 
{
    public class NewRelicContentManagerMessage : NewRelicInsightsMessage
    {
        public int ContentId { get; set; }
        public string ContentType { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; }
    }
}