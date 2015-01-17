using System;
using Orchard.Widgets.Models;

namespace Glimpse.Orchard.Models.Messages {
    public class WidgetMessage
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Zone { get; set; }
        public string Position { get; set; }
        public string TechnicalName { get; set; }
        public LayerPart Layer { get; set; }
        public TimeSpan Duration { get; set; }
    }
}