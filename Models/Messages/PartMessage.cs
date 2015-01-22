using System;
using System.Collections.Generic;
using Orchard.ContentManagement;

namespace Glimpse.Orchard.Models.Messages {
    public class PartMessage
    {
        public string ContentItemName { get; set; }
        public string ContentItemType { get; set; }
        public string ContentItemStereotype { get; set; }
        public int ContentItemId { get; set; }
        public string ContentPartType { get; set; }
        public IEnumerable<ContentField> Fields { get; set; }
        public TimeSpan Duration { get; set; }
    }
}