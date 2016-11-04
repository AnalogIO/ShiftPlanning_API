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
                .RegisterType<IShiftPlannerDataContext, ShiftPlannerDataContext>(new HierarchicalLifetimeManager())
                
                .RegisterType<IEmployeeRepository, EmployeeRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IEmployeeTitleRepository, EmployeeTitleRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IInstitutionRepository, InstitutionRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IManagerRepository, ManagerRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IScheduleRepository, ScheduleRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IShiftRepository, ShiftRepository>(new HierarchicalLifetimeManager());
        }
    }
}
