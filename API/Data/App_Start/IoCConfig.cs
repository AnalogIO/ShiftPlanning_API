using Data.Services;
using Microsoft.Practices.Unity;

namespace Data
{
    public static class IoCConfig
    {
        public static void ConfigureIoC(IUnityContainer container)
        {
            container
                .RegisterType<IEmployeeService, EmployeeService>(new PerResolveLifetimeManager())
                .RegisterType<IEmployeeTitleService, EmployeeTitleService>(new PerResolveLifetimeManager())
                .RegisterType<IPhotoService, PhotoService>(new PerResolveLifetimeManager())
                .RegisterType<IScheduleService, ScheduleService>(new PerResolveLifetimeManager())
                .RegisterType<IShiftService, ShiftService>(new PerResolveLifetimeManager())
                .RegisterType<IFriendshipService, FriendshipService>(new PerResolveLifetimeManager());
        }
    }
}
