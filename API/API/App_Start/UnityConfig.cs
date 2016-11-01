using System;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace API
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() => new UnityContainer());

        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = GetConfiguredContainer();

            Data.Npgsql.Configuration.IoCConfig.ConfigureIoC(container);

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}