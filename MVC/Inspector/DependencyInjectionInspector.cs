using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.Inspector
{
    public class DependencyInjectionInspector : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;

            var alternateImplementation = new AlternateType.DependencyResolver(context.ProxyFactory);

            var dependencyResolver = DependencyResolver.Current;

            IDependencyResolver newResolver;
            if (alternateImplementation.TryCreate(dependencyResolver, out newResolver))
            {
                DependencyResolver.SetResolver(newResolver);

                logger.Debug("Replaced IDependencyResolver of type '{0}' with proxy implementation.", dependencyResolver.GetType());
            }
            else
            {
                logger.Warn("Couldn't replace IDependencyResolver of type '{0}' with proxy implementation.", dependencyResolver.GetType());
            }
        }
    }
}