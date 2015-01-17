using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Models.Glimpse;

namespace Glimpse.Orchard.Tabs.EnabledFeatures
{
    public class EnabledFeaturesTab : TabBase, ITabSetup, IKey
    {
        public override object GetData(ITabContext context) 
        {
            var messages = context.GetMessages<GlimpseMessage<EnabledFeatureMessage>>().ToList();

            if (messages.Any())
            {
                return messages;
            }

            return "There is no data available for this tab, check that the 'Glimpse for Orchard Enabled Features' feature is enabled.";
        }

        public override string Name
        {
            get { return "Enabled Features"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<EnabledFeatureMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_enabledfeatures"; }
        }
    }

    public class EnabledFeatureMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<EnabledFeatureMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<EnabledFeatureMessage>> messages)
        {
            var root = new TabSection("Feature Type", "Category", "Name", "Description", "Id", "Priority");
            foreach (var message in messages.Unwrap().OrderBy(m => m.Extension.ExtensionType).ThenBy(m => m.Category).ThenBy(m => m.Name))
            {
                root.AddRow()
                    .Column(message.Extension.ExtensionType)
                    .Column(message.Category)
                    .Column(message.Name)
                    .Column(message.Description)
                    .Column(message.FeatureId)
                    .Column(message.Priority);
            }

            return root.Build();
        }
    }
}