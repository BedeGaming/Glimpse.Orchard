namespace Glimpse.Orchard.NewRelicInsights.Models.Messages 
{
    public class NewRelicInsightsMessage
    {
        // lower camel case to be compliant with New Relic's API
        // ReSharper disable once InconsistentNaming
        public string eventType { get; set; }
        // lower camel case to be compliant with New Relic's API
        // ReSharper disable once InconsistentNaming
        public long appId { get; set; }
        // lower camel case to be compliant with New Relic's API
        // ReSharper disable once InconsistentNaming
        //public long timestamp { get; set; }
        public string Tenant { get; set; }
    }
}