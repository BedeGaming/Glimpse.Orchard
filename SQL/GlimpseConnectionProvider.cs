using System.Web;
using NHibernate.Connection;
using NHibernate.Driver;

namespace Glimpse.Orchard.SQL
{
    public class GlimpseConnectionProvider : DriverConnectionProvider, IConnectionProvider
    {
        public new IDriver Driver
        {
            get
            {
                var originalDriver = base.Driver;

                if (HttpContext.Current == null || originalDriver is GlimpseDriver)
                {
                    return originalDriver;
                }

                return new GlimpseDriver(originalDriver);
            }
        }
    }
}