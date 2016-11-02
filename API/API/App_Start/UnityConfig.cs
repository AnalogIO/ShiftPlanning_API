using System;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace API
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() => { var container = new UnityContainer(); Data.Npgsql.Configuration.IoCConfig.ConfigureIoC(container); return container; });
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        public static void RegisterComponents(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityDependencyResolver(GetConfiguredContainer());
        }
    }
}