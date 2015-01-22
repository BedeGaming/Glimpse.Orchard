using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.Glimpse.Models;
using Glimpse.Orchard.Models.Messages;

namespace Glimpse.Orchard.Glimpse.Tabs.SiteSettings
{
    public class SiteSettingsTab : TabBase, ITabSetup, IKey
    {
        public override object GetData(ITabContext context) 
        {
            var messages = context.GetMessages<GlimpseMessage<SiteSettingMessage>>().ToList();

            if (messages.Any())
            {
                return messages;
            }

            return "There is no data available for this tab, check that the 'Glimpse for Orchard Site Settings' feature is enabled.";
        }

        public override string Name
        {
            get { return "Site Settings"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<SiteSettingMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_sitesettings"; }
        }
    }

    public class EnabledFeatureMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<SiteSettingMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<SiteSettingMessage>> messages)
        {
            var root = new TabSection("Part", "Name", "Value");
            foreach (var message in messages.Unwrap().OrderBy(m => m.Part).ThenBy(m => m.Name))
            {
                root.AddRow()
                    .Column(message.Part)
                    .Column(message.Name)
                    .Column(message.Value);
            }

            return root.Build();
        }
    }
}