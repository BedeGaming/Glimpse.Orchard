using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Models.Glimpse;

namespace Glimpse.Orchard.Tabs.Cache
{
    public class CacheTab : TabBase, ITabSetup, IKey
    {

        public override object GetData(ITabContext context)
        {
            return context.GetMessages<GlimpseMessage<CacheMessage>>();
        }

        public override string Name
        {
            get { return "Cache"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<CacheMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_cache"; }
        }
    }

    public class CacheMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<CacheMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<CacheMessage>> messages)
        {
            var root = new TabSection("Action", "Key", "Result", "Value", "Time Taken");
            foreach (var message in messages.Unwrap())
            {
                root.AddRow()
                    .Column(message.Action)
                    .Column(message.Key)
                    .Column(message.Result)
                    .Column(message.Value)
                    .Column(message.Duration.ToTimingString());
            }

            return root.Build();
        }
    }
}