using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Practices.Unity;

namespace DeadManSwitch.UI.Web.AspNetMvc
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

    }
}