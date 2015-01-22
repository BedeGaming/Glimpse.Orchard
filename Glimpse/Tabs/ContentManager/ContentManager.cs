using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.Glimpse.Models;
using Glimpse.Orchard.Models.Messages;

namespace Glimpse.Orchard.Glimpse.Tabs.ContentManager
{
    public class ContentManagerTab : TabBase, ITabSetup, IKey, ILayoutControl
    {
        public override object GetData(ITabContext context) {
            var messages = context.GetMessages<GlimpseMessage<ContentManagerMessage>>().ToList();

            if (!messages.Any())
            {
                return "There have been no Display Manager events recorded. If you think there should have been, check that the 'Glimpse for Orchard Content Manager' feature is enabled.";
            }

            return messages;
        }

        public override string Name
        {
            get { return "Content Manager"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<ContentManagerMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_contentmanager"; }
        }

        public bool KeysHeadings { get { return false; } }
    }

    public class ContentManagerGetMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<ContentManagerMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<ContentManagerMessage>> messages)
        {
            var root = new TabSection("Content Id", "Content Type", "Name", "Version Options", "Duration");
            foreach (var message in messages.Unwrap().OrderByDescending(m => m.Duration))
            {
                root.AddRow()
                    .Column(message.ContentId)
                    .Column(message.ContentType)
                    .Column(message.Name)
                    .Column(message.VersionOptions)
                    .Column(message.Duration.ToTimingString());
            }

            root.AddRow()
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