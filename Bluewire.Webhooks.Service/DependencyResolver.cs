using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace Bluewire.Webhooks.Service
{
    public class DependencyResolver : IDependencyResolver
    {
        public DependencyResolver()
        {
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return GetServices(serviceType).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            yield break;
        }
    }
}
