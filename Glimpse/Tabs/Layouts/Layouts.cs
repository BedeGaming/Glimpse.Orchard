using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.Glimpse.Models;
using Glimpse.Orchard.Models.Messages;

namespace Glimpse.Orchard.Glimpse.Tabs.Layouts
{
    public class LayoutTab : TabBase, ITabSetup, IKey
    {

        public override object GetData(ITabContext context)
        {
            var messages = context.GetMessages<GlimpseMessage<ElementMessage>>().ToList();

            if (!messages.Any())
            {
                return "There have been no Layout events recorded. If you think there should have been, check that the 'Glimpse for Orchard Layouts' feature is enabled.";
            }

            return messages;
        }

        public override string Name
        {
            get { return "Layouts"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<ElementMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_layouts"; }
        }
    }

    public class WidgetMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<ElementMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<ElementMessage>> messages)
        {
            var root = new TabSection("Category", "Element Type", "Index", "Rule", "HTML Class", "HTML Style", "HTML ID", "Is Container?", "Number Of Child Elements", "Duration");
            foreach (var message in messages.Unwrap().OrderBy(m=>m.Offset))
            {
                root.AddRow()
                    .Column(message.Category)
                    .Column(message.DisplayText)
                    .Column(message.Index)
                    .Column(message.Rule)
                    .Column(message.HtmlClass)
                    .Column(message.HtmlStyle)
                    .Column(message.HtmlId)
                    .Column(message.IsContainer ? "Yes" : "No")
                    .Column(message.IsContainer ? message.NumberOfChildElements.ToString() : null)
                    .Column(message.IsContainer ? null : message.Duration.ToTimingString());
            }

            root.AddRow()
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("Total time:")
                .Column(messages.Unwrap().Max(m => m.Duration.TotalMilliseconds).ToTimingString())
                .Selected();

            return root.Build();
        }
    }
}