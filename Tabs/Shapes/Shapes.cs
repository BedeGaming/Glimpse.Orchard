using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Models.Glimpse;

namespace Glimpse.Orchard.Tabs.Shapes
{
    public class ShapeTab : TabBase, ITabSetup, IKey
    {
        public override object GetData(ITabContext context)
        {
            var messages = context.GetMessages<GlimpseMessage<ShapeMessage>>().ToList();

            if (!messages.Any())
            {
                return "There have been no Shape events recorded. If you think there should have been, check that the 'Glimpse for Orchard Display Manager' feature is enabled.";
            }

            return messages;
        }

        public override string Name
        {
            get { return "Shapes"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<ShapeMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_shapes"; }
        }
    }

    public class ShapeMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<ShapeMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<ShapeMessage>> messages)
        {
            var root = new TabSection("Type", "DisplayType", "Position", "Placement Source", "Prefix", "Binding Source", "Available Binding Sources", "Wrappers", "Alternates", "Build Display Duration");
            foreach (var message in messages.Unwrap().OrderByDescending(m=>m.Duration.TotalMilliseconds)) {
                if (message.Type != "Layout" //these exemptions are taken from the Shape Tracing Feature
                    && message.Type != "DocumentZone"
                    && message.Type != "PlaceChildContent"
                    && message.Type != "ContentZone"
                    && message.Type != "ShapeTracingMeta"
                    && message.Type != "ShapeTracingTemplates"
                    && message.Type != "DateTimeRelative")
                {
                    root.AddRow()
                        .Column(message.Type)
                        .Column(message.DisplayType)
                        .Column(message.Position)
                        .Column(message.PlacementSource)
                        .Column(message.Prefix)
                        .Column(message.BindingSource)
                        .Column(message.BindingSources)
                        .Column(message.Wrappers)
                        .Column(message.Alternates)
                        .Column(message.Duration.ToTimingString());
                }
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
                .Column("Total time (includes nested times):")
                .Column(messages.Unwrap().Sum(m => m.Duration.TotalMilliseconds).ToTimingString())
                .Selected();

            return root.Build();
        }
    }
}