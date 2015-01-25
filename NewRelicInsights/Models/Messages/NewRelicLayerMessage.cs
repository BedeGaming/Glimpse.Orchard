namespace Glimpse.Orchard.NewRelicInsights.Models.Messages 
{
    public class NewRelicLayerMessage : NewRelicInsightsMessage
    {
        public string LayerName { get; set; }
        public string LayerRule { get; set; }
        public int Active { get; set; }
        public double Duration { get; set; }
    }
}