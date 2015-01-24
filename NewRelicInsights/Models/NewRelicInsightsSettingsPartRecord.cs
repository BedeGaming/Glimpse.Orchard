using Orchard.ContentManagement.Records;

namespace Glimpse.Orchard.NewRelicInsights.Models
{
    public class NewRelicInsightsSettingsPartRecord : ContentPartRecord
    {
        public virtual string InsertKey { get; set; }
        public virtual long AccountId { get; set; }
        public virtual long AppId { get; set; }
        public virtual int BufferSize { get; set; }
    }
}