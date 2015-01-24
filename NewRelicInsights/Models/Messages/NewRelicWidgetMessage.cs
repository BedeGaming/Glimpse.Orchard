namespace Glimpse.Orchard.NewRelicInsights.Models.Mesages {
    public class NewRelicWidgetMessage : NewRelicInsightsMessage
    {
        public string WidgetName { get; set; }
        public string WidgetType { get; set; }
        public string Zone { get; set; }
        public string Layer { get; set; }
        public double Duration { get; set; }
    }
}