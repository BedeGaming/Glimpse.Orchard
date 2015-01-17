using System;

namespace Glimpse.Orchard.Tabs.Authorizer {
    public class AuthorizerMessage
    {
        public string PermissionName { get; set; }
        public bool UserIsAuthorized { get; set; }
        public int ContentId { get; set; }
        public string ContentName { get; set; }
        public string ContentType { get; set; }
        public TimeSpan Duration { get; set; }
    }
}