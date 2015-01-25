namespace Glimpse.Orchard.NewRelicInsights.Models.Messages 
{
    public class NewRelicCacheMessage : NewRelicInsightsMessage
    {
        public string Key { get; set; }
        public string Action { get; set; }
        public string Result { get; set; }
        public double Duration { get; set; }
    }
}