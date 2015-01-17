using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Models.Glimpse;

namespace Glimpse.Orchard.Tabs.Parts
{
    public class PartTab : TabBase, ITabSetup, IKey
    {
        public override object GetData(ITabContext context)
        {
            var messages = context.GetMessages<GlimpseMessage<PartMessage>>().ToList();

            if (!messages.Any())
            {
                return "There have been no Content Part Driver events recorded. If you think there should have been, check that the 'Glimpse for Orchard Content Part Drivers' feature is enabled.";
            }

            return messages;
        }

        public override string Name
        {
            get { return "Part Drivers"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<PartMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_parts"; }
        }

    }

    public class PartMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<PartMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<PartMessage>> messages)
        {
            var root = new TabSection("Content Item Name", "Content Item Type", "Content Item Stereotype", "Content Part", "Fields", "Duration");
            foreach (var message in messages.Unwrap().OrderByDescending(m=>m.Duration))
            {
                root.AddRow()
                    .Column(message.ContentItemName)
                    .Column(message.ContentItemType)
                    .Column(message.ContentItemStereotype)
                    .Column(message.ContentPartType)
                    .Column(message.Fields.Any() ? message.Fields as object : "None")
                    .Column(message.Duration.ToTimingString());
            }

            root.AddRow()
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("Total time:")
                .Column(messages.Unwrap().Sum(m => m.Duration.TotalMilliseconds).ToTimingString())
                .Selected();

            return root.Build();
        }
    }
}