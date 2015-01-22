using System;

namespace Glimpse.Orchard.Models.Messages {
    public class CacheMessage
    {
        public string Action { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public string Result { get; set; }
        public TimeSpan ValidFor { get; set; }
        public TimeSpan Duration { get; set; }
    }
}