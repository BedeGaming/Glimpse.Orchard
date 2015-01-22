using Orchard.Environment.Extensions.Models;

namespace Glimpse.Orchard.Models.Messages {
    public class EnabledFeatureMessage
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string FeatureId { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public ExtensionDescriptor Extension { get; set; }
    }
}