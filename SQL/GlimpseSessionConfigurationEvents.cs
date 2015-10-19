using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Utility;

namespace Glimpse.Orchard.SQL
{
    [OrchardFeature("Glimpse.Orchard.SQL")]
    public class GlimpseSessionConfigurationEvents : ISessionConfigurationEvents
    {
        public void Created(FluentConfiguration cfg, AutoPersistenceModel defaultModel) { }

        public void Prepared(FluentConfiguration cfg) { }

        public void Building(Configuration cfg) { }

        public void Finished(Configuration cfg)
        {
            cfg.SetProperty("connection.provider", "Glimpse.Orchard.SQL.GlimpseConnectionProvider, Glimpse.Orchard");
        }

        public void ComputingHash(Hash hash) { }
    }
}