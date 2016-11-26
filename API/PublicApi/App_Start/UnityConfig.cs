using System;
using System.Threading;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;

namespace PublicApi
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            IoCConfig.ConfigureIoC(container);
            return container;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        public static void RegisterComponents(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityHierarchicalDependencyResolver(GetConfiguredContainer());
        }
    }
}
