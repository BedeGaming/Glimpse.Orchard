using System;
using Orchard.ContentManagement;

namespace Glimpse.Orchard.Models.Messages
{
    public class ElementMessage
    {
        public TimeSpan Duration { get; set; }
        public TimeSpan Offset { get; set; }
        public IContent ContentItem { get; set; }
        public string DisplayText { get; set; }
        public string Category { get; set; }
        public string HtmlId { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlStyle { get; set; }
        public string Rule { get; set; }
        public int Index { get; set; }
        public bool IsContainer { get; set; }
        public int NumberOfChildElements { get; set; }
    }
}