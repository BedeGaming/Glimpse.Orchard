using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Script.Serialization;
using Glimpse.Orchard.MessageBrokers;
using Glimpse.Orchard.NewRelicInsights.MessageTransformers;
using Glimpse.Orchard.NewRelicInsights.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;

namespace Glimpse.Orchard.NewRelicInsights.MessageBrokers
{
    public class NewRelicInsightsMessageBroker : IPerformanceMessageBroker, ISingletonDependency
    {
        private readonly ShellSettings _shellSettings;
        private readonly IEnumerable<INewRelicInsightsMessageTransformer> _messageTransformers;
        private readonly Lazy<IOrchardServices> _orchardServices;
        private readonly ICollection<object> _messages;

        private NewRelicInsightsSettingsPart Settings { get; set; }
        private bool SettingsAreValid { get; set; }

        public NewRelicInsightsMessageBroker(ShellSettings shellSettings, IEnumerable<INewRelicInsightsMessageTransformer> messageTransformers, Lazy<IOrchardServices> orchardServices) {
            _shellSettings = shellSettings;
            _messageTransformers = messageTransformers;
            _orchardServices = orchardServices;
            _messages = new Collection<object>();
        }

        public void Publish<T>(T message)
        {
            Settings = _orchardServices.Value.WorkContext.CurrentSite.As<NewRelicInsightsSettingsPart>();
            SettingsAreValid = Settings != null;

            if (SettingsAreValid) 
            {
                foreach (var messageTransformer in _messageTransformers) {
                    var transformedMessage = messageTransformer.TransformMessage(message);

                    if (transformedMessage != null) {
                        transformedMessage.eventType = messageTransformer.EventName;
                        transformedMessage.appId = Settings.AppId;
                        transformedMessage.Tenant = _shellSettings.Name;

                        _messages.Add(transformedMessage);
                        break;
                    }
                }


                if (_messages.Count == Settings.BufferSize) 
                {
                    var client = new HttpClient {
                        BaseAddress = new Uri("https://insights-collector.newrelic.com/v1/accounts/" + Settings.AccountId + "/events")
                    };

                    client.DefaultRequestHeaders.Add("X-Insert-Key", Settings.InsertKey);
                    var json = new StringContent(new JavaScriptSerializer().Serialize(_messages));

                    client.PostAsync("", json);

                    _messages.Clear();
                }
            }
        }

        public Guid Subscribe<T>(Action<T> action) {
            return Guid.NewGuid();
        }

        public void Unsubscribe<T>(Guid subscriptionId) {}
    }
}