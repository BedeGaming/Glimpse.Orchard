using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.Orchard.Extensions;
using Glimpse.Orchard.Glimpse.Extensions;
using Glimpse.Orchard.Glimpse.Models;
using Glimpse.Orchard.Models.Messages;

namespace Glimpse.Orchard.Glimpse.Tabs.Authorizer
{
    public class AuthorizerManagerTab : TabBase, ITabSetup, IKey
    {
        public override object GetData(ITabContext context) 
        {
            var messages = context.GetMessages<GlimpseMessage<AuthorizerMessage>>().ToList();

            if (messages.Any())
            {
                return messages;
            }

            return "There have been no Authorization events recorded. If you think there should have been, check that the 'Glimpse for Orchard Authorizer' feature is enabled.";
        }

        public override string Name
        {
            get { return "Authorizer"; }
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<GlimpseMessage<AuthorizerMessage>>();
        }

        public string Key
        {
            get { return "glimpse_orchard_authorizer"; }
        }
    }

    public class AuthorizerMessagesConverter : SerializationConverter<IEnumerable<GlimpseMessage<AuthorizerMessage>>>
    {
        public override object Convert(IEnumerable<GlimpseMessage<AuthorizerMessage>> messages)
        {
            var root = new TabSection("Permission Name", "User is Authorized", "Content Id", "Content Name", "Content Type", "Evaluation Time");
            foreach (var message in messages.Unwrap().OrderByDescending(m => m.Duration))
            {
                root.AddRow()
                    .Column(message.PermissionName)
                    .Column(message.UserIsAuthorized ? "Yes" : "No")
                    .Column((message.ContentId == 0 ? null : message.ContentId.ToString()))
                    .Column(message.ContentName)
                    .Column(message.ContentType)
                    .Column(message.Duration.ToTimingString())
                    .QuietIf(!message.UserIsAuthorized);
            }

            root.AddRow()
                .Column("")
                .Column("")
                .Column("")
                .Column("")
                .Column("Total time:")
                .Column(messages.Unwrap().Sum(m=>m.Duration.TotalMilliseconds).ToTimingString())
                .Selected();

            return root.Build();
        }
    }
}