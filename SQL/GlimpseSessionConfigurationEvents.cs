using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate.AdoNet;
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
            cfg.SetProperty(Environment.ConnectionProvider, "Glimpse.Orchard.SQL.GlimpseConnectionProvider, Glimpse.Orchard");
            cfg.SetProperty(Environment.BatchStrategy, typeof(NonBatchingBatcherFactory).FullName);
            cfg.SetProperty(Environment.FormatSql, "true");
        }

        public void ComputingHash(Hash hash) { }
    }
}