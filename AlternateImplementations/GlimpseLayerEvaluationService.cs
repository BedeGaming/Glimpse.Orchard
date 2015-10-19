using System;
using System.Collections.Generic;
using Glimpse.Orchard.Models;
using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.PerformanceMonitors;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;

namespace Glimpse.Orchard.AlternateImplementations
{
    [OrchardFeature("Glimpse.Orchard.Widgets")]
    [OrchardSuppressDependency("Orchard.Widgets.Services.DefaultLayerEvaluationService")]
    public class GlimpseLayerEvaluationService : ILayerEvaluationService
    {
        private readonly IRuleManager _ruleManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IPerformanceMonitor _performanceMonitor;

        public GlimpseLayerEvaluationService(IRuleManager ruleManager, IPerformanceMonitor performanceMonitor, IOrchardServices orchardServices)
        {
            _ruleManager = ruleManager;
            _performanceMonitor = performanceMonitor;
            _orchardServices = orchardServices;

            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; private set; }

        public int[] GetActiveLayerIds()
        {
            // Once the Rule Engine is done:
            // Get Layers and filter by zone and rule
            // NOTE: .ForType("Layer") is faster than .Query<LayerPart, LayerPartRecord>()
            var activeLayers = _orchardServices.ContentManager.Query<LayerPart>().WithQueryHints(new QueryHints().ExpandParts<LayerPart>()).ForType("Layer").List();

            var activeLayerIds = new List<int>();
            foreach (var activeLayer in activeLayers)
            {
                // ignore the rule if it fails to execute
                try
                {
                    var currentLayer = activeLayer;
                    var layerRuleMatches = _performanceMonitor.PublishTimedAction(() => _ruleManager.Matches(currentLayer.Record.LayerRule), (r, t) => new LayerMessage
                    {
                        Active = r,
                        Name = currentLayer.Record.Name,
                        Rule = currentLayer.Record.LayerRule,
                        Duration = t.Duration
                    }, TimelineCategories.Layers, "Layer Evaluation", currentLayer.Record.Name).ActionResult;

                    if (layerRuleMatches)
                    {
                        activeLayerIds.Add(activeLayer.ContentItem.Id);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warning(e, T("An error occured during layer evaluation on: {0}", activeLayer.Name).Text);
                }
            }

            return activeLayerIds.ToArray();
        }
    }
}