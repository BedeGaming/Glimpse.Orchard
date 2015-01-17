using System;
using Orchard.ContentManagement;

namespace Glimpse.Orchard.Models.Messages {
    public class ContentManagerMessage
    {
        public int ContentId { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public VersionOptions VersionOptions { get; set; }
        public TimeSpan Duration { get; set; }
    }
}