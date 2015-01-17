using Glimpse.Core.Message;

namespace Glimpse.Orchard.Glimpse.Models
{
    public class GlimpseMessage<T> : MessageBase
    {
        public GlimpseMessage(T payload) {
            Payload = payload;
        }
        public T Payload { get; set; }
    }
}