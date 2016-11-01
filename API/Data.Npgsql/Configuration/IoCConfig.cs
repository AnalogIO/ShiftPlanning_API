using Data.Npgsql.Mapping;
using Data.Npgsql.Models;
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
                
                .RegisterType<IMapper<Data.Models.Employee, Employee>, EmployeeMapper>(new HierarchicalLifetimeManager())
                .RegisterType<IMapper<Data.Models.EmployeeTitle, EmployeeTitle>, EmployeeTitleMapper>(new HierarchicalLifetimeManager())
                .RegisterType<IMapper<Data.Models.Institution, Institution>, InstitutionMapper>(new HierarchicalLifetimeManager())
                .RegisterType<IMapper<Data.Models.Manager, Manager>, ManagerMapper>(new HierarchicalLifetimeManager())
                .RegisterType<IMapper<Data.Models.ScheduledShift, ScheduledShift>, ScheduledShiftMapper>(new HierarchicalLifetimeManager())
                .RegisterType<IMapper<Data.Models.Schedule, Schedule>, ScheduleMapper>(new HierarchicalLifetimeManager())
                .RegisterType<IMapper<Data.Models.Shift, Shift>, ShiftMapper>(new HierarchicalLifetimeManager())
                
                .RegisterType<IMapMany<CheckIn, Data.Models.CheckIn>, CheckInMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<Employee, Data.Models.Employee>, EmployeeMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<EmployeeTitle, Data.Models.EmployeeTitle>, EmployeeTitleMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<Manager, Data.Models.Manager>, ManagerMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<Schedule, Data.Models.Schedule>, ScheduleMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<ScheduledShift, Data.Models.ScheduledShift>, ScheduledShiftMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<Shift, Data.Models.Shift>, ShiftMapMany>(new HierarchicalLifetimeManager())
                .RegisterType<IMapMany<Models.Token, Data.Models.Token>, TokenMapMany>(new HierarchicalLifetimeManager())
                
                .RegisterType<IEmployeeRepository, EmployeeRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IEmployeeTitleRepository, EmployeeTitleRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IInstitutionRepository, InstitutionRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IManagerRepository, ManagerRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IScheduleRepository, ScheduleRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IShiftRepository, ShiftRepository>(new HierarchicalLifetimeManager());
        }
    }
}
