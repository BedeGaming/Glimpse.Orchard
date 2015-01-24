using Glimpse.Orchard.NewRelicInsights.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Glimpse.Orchard.NewRelicInsights.Handlers
{
    public class NewRelicInsightsSettingsPartHandler : ContentHandler
    {
        public NewRelicInsightsSettingsPartHandler(IRepository<NewRelicInsightsSettingsPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<NewRelicInsightsSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));   
        }
    }
}