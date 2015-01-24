using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace Glimpse.Orchard.NewRelicInsights.Models 
{
    public class NewRelicInsightsSettingsPart : ContentPart<NewRelicInsightsSettingsPartRecord>
    {
        public string InsertKey
        {
            get { return Record.InsertKey; }
            set { Record.InsertKey = value; }
        }

        public long AccountId
        {
            get { return Record.AccountId; }
            set { Record.AccountId = value; }
        }

        public long AppId
        {
            get { return Record.AppId; }
            set { Record.AppId = value; }
        }

        [Range(1, 1000, ErrorMessage = "Please select a positive Buffer Size under the size of 1000 (1000 is the limit that the New Relic API supports)")]
        public int BufferSize
        {
            get { return Record.BufferSize; }
            set { Record.BufferSize = value; }
        }
    }
}