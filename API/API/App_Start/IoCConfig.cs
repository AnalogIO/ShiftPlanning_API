using API.Authorization;
using Data.Services;
using Microsoft.Practices.Unity;

namespace API
{
    /// <summary>
    /// API specific Inversion of Control setup.
    /// </summary>
    public static class IoCConfig
    {
        /// <summary>
        /// Sets up the container with required mappings for API.
        /// </summary>
        /// <param name="container">The Microsoft Unity container to register types on.</param>
        public static void ConfigureIoC(IUnityContainer container)
        {
            Data.Npgsql.Configuration.IoCConfig.ConfigureIoC(container);
            Data.IoCConfig.ConfigureIoC(container);

            container
                .RegisterType<IAuthManager, AuthManager>(new PerResolveLifetimeManager());
        }
    }
}