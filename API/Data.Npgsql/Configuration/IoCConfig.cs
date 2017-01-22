using Data.Npgsql.Repositories;
using Data.Repositories;
using Microsoft.Practices.Unity;

namespace Data.Npgsql.Configuration
{
    public static class IoCConfig
    {
        public static void ConfigureIoC(IUnityContainer container)
        {
            container
                .RegisterType<IShiftPlannerDataContext, ShiftPlannerDataContext>(new PerResolveLifetimeManager())
                
                .RegisterType<IPhotoRepository, PhotoRepository>(new PerResolveLifetimeManager())
                .RegisterType<IEmployeeRepository, EmployeeRepository>(new PerResolveLifetimeManager())
                .RegisterType<IEmployeeTitleRepository, EmployeeTitleRepository>(new PerResolveLifetimeManager())
                .RegisterType<IOrganizationRepository, OrganizationRepository>(new PerResolveLifetimeManager())
                .RegisterType<IManagerRepository, ManagerRepository>(new PerResolveLifetimeManager())
                .RegisterType<IScheduleRepository, ScheduleRepository>(new PerResolveLifetimeManager())
                .RegisterType<IShiftRepository, ShiftRepository>(new PerResolveLifetimeManager());
        }
    }
}
