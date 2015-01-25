namespace Glimpse.Orchard.NewRelicInsights.Models 
{
    public class NewRelicInsightsSettingsPart 
    {
        public string InsertKey { get; set; }
        public long AccountId { get; set; }
        public long AppId { get; set; }
        public int BufferSize { get; set; }
    }
}