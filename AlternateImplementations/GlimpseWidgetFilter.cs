using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Orchard.Models;
using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.PerformanceMonitors;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Settings.Models;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Filters;
using Orchard.Themes;
using Orchard.UI.Admin;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using ILogger = Orchard.Logging.ILogger;
using NullLogger = Orchard.Logging.NullLogger;

namespace Glimpse.Orchard.AlternateImplementations
{
    [OrchardFeature("Glimpse.Orchard.Widgets")]
    [OrchardSuppressDependency("Orchard.Widgets.Filters.WidgetFilter")]
    public class GlimpseWidgetFilter : FilterProvider, IResultFilter
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IWidgetsService _widgetsService;
        private readonly IOrchardServices _orchardServices;
        private readonly IPerformanceMonitor _performanceMonitor;
        private readonly ILayerEvaluationService _layerEvaluationService;

        public GlimpseWidgetFilter(
            IWorkContextAccessor workContextAccessor,
            IWidgetsService widgetsService,
            IOrchardServices orchardServices,
            IPerformanceMonitor performanceMonitor, 
            ILayerEvaluationService layerEvaluationService)
        {
            _workContextAccessor = workContextAccessor;
            _widgetsService = widgetsService;
            _orchardServices = orchardServices;
            _performanceMonitor = performanceMonitor;
            _layerEvaluationService = layerEvaluationService;

            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; private set; }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // layers and widgets should only run on a full view rendering result
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult == null)
                return;

            var workContext = _workContextAccessor.GetContext(filterContext.HttpContext);

            if (workContext == null ||
                workContext.Layout == null ||
                workContext.CurrentSite == null ||
                AdminFilter.IsApplied(filterContext.RequestContext) ||
                !ThemeFilter.IsApplied(filterContext.RequestContext))
            {
                return;
            }

            IEnumerable<WidgetPart> widgetParts = _widgetsService.GetWidgets(_layerEvaluationService.GetActiveLayerIds().ToArray());

            // Build and add shape to zone.
            var zones = workContext.Layout.Zones;
            var defaultCulture = workContext.CurrentSite.As<SiteSettingsPart>().SiteCulture;
            var currentCulture = workContext.CurrentCulture;

            foreach (var widgetPart in widgetParts)
            {
                var commonPart = widgetPart.As<ICommonPart>();
                if (commonPart == null || commonPart.Container == null)
                {
                    Logger.Warning("The widget '{0}' is has no assigned layer or the layer does not exist.", widgetPart.Title);
                    continue;
                }

                // ignore widget for different cultures
                var localizablePart = widgetPart.As<ILocalizableAspect>();
                if (localizablePart != null)
                {
                    // if localized culture is null then show if current culture is the default
                    // this allows a user to show a content item for the default culture only
                    if (localizablePart.Culture == null && defaultCulture != currentCulture)
                    {
                        continue;
                    }

                    // if culture is set, show only if current culture is the same
                    if (localizablePart.Culture != null && localizablePart.Culture != currentCulture)
                    {
                        continue;
                    }
                }

                // check permissions
                if (!_orchardServices.Authorizer.Authorize(global::Orchard.Core.Contents.Permissions.ViewContent, widgetPart))
                {
                    continue;
                }

                var scopedWidgetPart = widgetPart;
                var widgetShape = _performanceMonitor.PublishTimedAction(() => _orchardServices.ContentManager.BuildDisplay(scopedWidgetPart), (r, t) =>
                    new WidgetMessage
                    {
                        Title = scopedWidgetPart.Title,
                        Type = scopedWidgetPart.ContentItem.ContentType,
                        Zone = scopedWidgetPart.Zone,
                        Layer = scopedWidgetPart.LayerPart,
                        Position = scopedWidgetPart.Position,
                        TechnicalName = scopedWidgetPart.Name,
                        Duration = t.Duration
                    }, TimelineCategories.Widgets, string.Format("Build Display ({0})", scopedWidgetPart.ContentItem.ContentType), scopedWidgetPart.Title);

                zones[widgetPart.Record.Zone].Add(widgetShape.ActionResult, widgetPart.Record.Position);
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
