using Microsoft.Practices.Unity;
using PublicApi.Mapping;

namespace PublicApi
{
    public static class IoCConfig
    {
        public static void ConfigureIoC(IUnityContainer container)
        {
            Data.Npgsql.Configuration.IoCConfig.ConfigureIoC(container);
            Data.IoCConfig.ConfigureIoC(container);

            container
                .RegisterType<IOpeningHoursMapper, OpeningHoursMapper>(new ContainerControlledLifetimeManager())
                .RegisterType<IVolunteerMapper, EmployeeMapper>(new ContainerControlledLifetimeManager());
        }
    }
}