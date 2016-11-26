using Data.Services;
using Microsoft.Practices.Unity;

namespace Data
{
    public static class IoCConfig
    {
        public static void ConfigureIoC(IUnityContainer container)
        {
            container.RegisterType<IEmployeeService, EmployeeService>(new HierarchicalLifetimeManager())
                .RegisterType<IEmployeeTitleService, EmployeeTitleService>(new HierarchicalLifetimeManager())
                .RegisterType<IScheduleService, ScheduleService>(new HierarchicalLifetimeManager())
                .RegisterType<IShiftService, ShiftService>(new HierarchicalLifetimeManager());
        }
    }
}
