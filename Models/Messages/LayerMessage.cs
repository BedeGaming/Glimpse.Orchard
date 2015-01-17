using System;

namespace Glimpse.Orchard.Models.Messages {
    public class LayerMessage
    {
        public string Name { get; set; }
        public string Rule { get; set; }
        public bool Active { get; set; }
        public TimeSpan Duration { get; set; }
    }
}