using System.Collections.Generic;
using System.Linq;
using Glimpse.Orchard.Glimpse.Models;

namespace Glimpse.Orchard.Glimpse.Extensions
{
    public static class GlimpseMessageExtensions
    {
        public static IEnumerable<T> Unwrap<T>(this IEnumerable<GlimpseMessage<T>> messages) {
            return messages.Select(m => m.Payload);
        }
    }
}