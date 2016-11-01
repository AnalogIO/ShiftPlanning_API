using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class ScheduledShiftMapper : IMapper<Data.Models.ScheduledShift, ScheduledShift>
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapMany<Employee, Data.Models.Employee> _employeeMapMany;

        public ScheduledShiftMapper(IShiftPlannerDataContext context,
            IMapMany<Employee, Data.Models.Employee> employeeMapMany)
        {
            _context = context;
            _employeeMapMany = employeeMapMany;
        }

        public ScheduledShift MapToEntity(Data.Models.ScheduledShift model)
        {
            return new ScheduledShift
            {
                Day = model.Day,
                Start = model.Start,
                End = model.End,
                Employees = model.Employees.Select(e => _context.Employees.Single(em => em.Id == e.Id)).ToList()
            };
        }

        public Data.Models.ScheduledShift MapToModel(ScheduledShift entity)
        {
            return new Data.Models.ScheduledShift
            {
                Id = entity.Id,
                Day = entity.Day,
                Start = entity.Start,
                End = entity.End,
                Employees = _employeeMapMany.Map(entity.Employees)
            };
        }
    }
}
