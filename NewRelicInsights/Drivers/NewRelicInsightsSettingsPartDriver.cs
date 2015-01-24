using Glimpse.Orchard.NewRelicInsights.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Glimpse.Orchard.NewRelicInsights.Drivers 
{
    public class NewRelicInsightsSettingsPartDriver : ContentPartDriver<NewRelicInsightsSettingsPart>
    {
        protected override string Prefix {
            get { return "NewRelicInsightsSettingsPart"; }
        }

        protected override DriverResult Editor(NewRelicInsightsSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_NewRelicInsightsSettingsPart_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/NewRelicInsightsSettingsPart",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(NewRelicInsightsSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}