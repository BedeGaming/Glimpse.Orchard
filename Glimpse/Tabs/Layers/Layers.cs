using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.Glimpse.Models;
using Glimpse.Orchard.Models.Messages;

namespace Glimpse.Orchard.Glimpse.Tabs.Layers
{
    public class LayerTab : TabBase, ITabSetup, IKey
    {
        public override object GetData(ITabContext context) 
        {
            var messages = context.GetMessages<GlimpseMessage<LayerMessage>>().ToList();

            if (!messages.Any()) {
                return "There have been no Layer events recorded. If you think there should have been, check that the 'Glimpse for Orchard Widgets' feature is enabled.";
            }

            return messages;
        }

        public override string Name
        {
            get { return "Layers"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<LayerMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_layers"; }
        }
    }

    public class LayerMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<LayerMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<LayerMessage>> messages)
        {
            var root = new TabSection("Layer Name", "Layer Rule", "Active", "Evaluation Time");
            foreach (var message in messages.Unwrap().OrderByDescending(m=>m.Duration))
            {
                root.AddRow()
                    .Column(message.Name)
                    .Column(message.Rule)
                    .Column(message.Active ? "Yes" : "No")
                    .Column(message.Duration.ToTimingString())
                    .QuietIf(!message.Active);
            }

            root.AddRow()
                .Column("")
                .Column("")
                .Column("Total time:")
                .Column(messages.Unwrap().Sum(m => m.Duration.TotalMilliseconds).ToTimingString())
                .Selected();

            return root.Build();
        }
    }
}