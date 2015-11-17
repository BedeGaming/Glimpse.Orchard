using Orchard.Layouts.Framework.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Orchard.Models;
using Glimpse.Orchard.Models.Messages;
using Glimpse.Orchard.PerformanceMonitors;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.Services;
using Orchard.Layouts.Elements;

namespace Glimpse.Orchard.AlternateImplementations
{
    [OrchardFeature("Glimpse.Orchard.Layouts")]
    [OrchardSuppressDependency("Orchard.Layouts.Framework.Display.ElementDisplay")]
    public class GlimpseElementDisplay : ElementDisplay, IElementDisplay
    {
        private readonly IShapeFactory _shapeFactory;
        private readonly IPerformanceMonitor _performanceMonitor;

        public GlimpseElementDisplay(IShapeFactory shapeFactory, IElementEventHandler elementEventHandlerHandler, IPerformanceMonitor performanceMonitor)
            : base(shapeFactory, elementEventHandlerHandler)
        {
            _shapeFactory = shapeFactory;
            _performanceMonitor = performanceMonitor;
        }

        public override dynamic DisplayElement(
            Element element,
            IContent content,
            string displayType = null,
            IUpdateModel updater = null)
        {
            var container = element as Container;

            return _performanceMonitor.PublishTimedAction(() =>
            {
                return base.DisplayElement(element, content, displayType, updater);
            }, (r, t) => new ElementMessage()
            {
                Duration = t.Duration,
                Offset = t.Offset,
                ContentItem = content,
                Category = element.Category,
                DisplayText = element.DisplayText.Text,
                HtmlClass = element.HtmlClass,
                HtmlId = element.HtmlId,
                HtmlStyle = element.HtmlStyle,
                Index = element.Index,
                Rule = element.Rule,
                NumberOfChildElements = container == null ? 0 : container.Elements.Count(),
                IsContainer = container!= null
            }, TimelineCategories.Layouts, "Element Displaying", element.DisplayText.Text).ActionResult;
        }
    }
}