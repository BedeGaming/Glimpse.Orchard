using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net.Http;
using System.Web.Script.Serialization;
using Glimpse.Orchard.MessageBrokers;
using Glimpse.Orchard.NewRelicInsights.MessageTransformers;
using Glimpse.Orchard.NewRelicInsights.Models;
using Orchard;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Configuration;
using Orchard.Environment.Extensions;

namespace Glimpse.Orchard.NewRelicInsights.MessageBrokers
{
    [OrchardFeature("Glimpse.Orchard.NewRelicInsights")]
    public class NewRelicInsightsMessageBroker : IPerformanceMessageBroker, ISingletonDependency
    {
        private readonly ShellSettings _shellSettings;
        private readonly IEnumerable<INewRelicInsightsMessageTransformer> _messageTransformers;
        private readonly ICollection<object> _messages;

        private LazyField<NewRelicInsightsSettingsPart> Settings { get; set; }
        private bool SettingsAreValid { get; set; }

        public NewRelicInsightsMessageBroker(ShellSettings shellSettings, IEnumerable<INewRelicInsightsMessageTransformer> messageTransformers)
        {
            _shellSettings = shellSettings;
            _messageTransformers = messageTransformers;
            _messages = new Collection<object>();

            Settings = new LazyField<NewRelicInsightsSettingsPart>();
            Settings.Loader(() => 
            {
                var settings = new NewRelicInsightsSettingsPart
                {
                    AccountId = GetLongConfig("Glimpse.Orchard.NewRelicInsights.AccountId"),
                    AppId = GetLongConfig("Glimpse.Orchard.NewRelicInsights.AppId"),
                    InsertKey = ConfigurationManager.AppSettings["Glimpse.Orchard.NewRelicInsights.InsertKey"],
                    BufferSize = GetIntConfig("Glimpse.Orchard.NewRelicInsights.BufferSize"),
                };

                SettingsAreValid = !string.IsNullOrEmpty(settings.InsertKey)
                    && settings.AccountId > 0
                    && settings.BufferSize > 0
                    && settings.BufferSize <= 1000;

                return settings;
            });

        }

        public void Publish<T>(T message)
        {
            var settings = Settings.Value;
            if (SettingsAreValid)
            {
                foreach (var messageTransformer in _messageTransformers)
                {
                    var transformedMessage = messageTransformer.TransformMessage(message);

                    if (transformedMessage != null)
                    {
                        transformedMessage.eventType = messageTransformer.EventName;
                        transformedMessage.appId = settings.AppId;
                        transformedMessage.Tenant = _shellSettings.Name;

                        _messages.Add(transformedMessage);
                        break;
                    }
                }


                if (_messages.Count == settings.BufferSize)
                {
                    var client = new HttpClient
                    {
                        BaseAddress = new Uri("https://insights-collector.newrelic.com/v1/accounts/" + settings.AccountId + "/events")
                    };

                    client.DefaultRequestHeaders.Add("X-Insert-Key", settings.InsertKey);
                    var json = new StringContent(new JavaScriptSerializer().Serialize(_messages));

                    client.PostAsync("", json);

                    _messages.Clear();
                }
            }
        }

        private long GetLongConfig(string key)
        {
            long result;
            long.TryParse(ConfigurationManager.AppSettings[key], out result);

            return result;
        }

        private int GetIntConfig(string key)
        {
            int result;
            int.TryParse(ConfigurationManager.AppSettings[key], out result);

            return result;
        }
    }
}