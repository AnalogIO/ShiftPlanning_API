using API.Authorization;
using API.Logic;
using API.Mapping;
using API.Services;
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
            container

                // Container controlled since it has no dependencies/state.
                .RegisterType<IOpeningHoursMapper, OpeningHoursMapper>(new ContainerControlledLifetimeManager())

                .RegisterType<IAuthManager, AuthManager>(new HierarchicalLifetimeManager())

                .RegisterType<IEmployeeService, EmployeeService>(new HierarchicalLifetimeManager())
                .RegisterType<IScheduleService, ScheduleService>(new HierarchicalLifetimeManager());
        }
    }
}