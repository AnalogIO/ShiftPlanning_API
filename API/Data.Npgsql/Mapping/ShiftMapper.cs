using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class ShiftMapper : IMapper<Data.Models.Shift, Shift>
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Data.Models.Institution, Institution> _institutionMapper;
        private readonly IMapMany<CheckIn, Data.Models.CheckIn> _checkInMapMany;
        private readonly IMapMany<Employee, Data.Models.Employee> _employeeMapMany;

        public ShiftMapper(IShiftPlannerDataContext context, 
            IMapper<Data.Models.Institution, Institution> institutionMapper, 
            IMapMany<CheckIn, Data.Models.CheckIn> checkInMapMany, 
            IMapMany<Employee, Data.Models.Employee> employeeMapMany)
        {
            _context = context;
            _institutionMapper = institutionMapper;
            _checkInMapMany = checkInMapMany;
            _employeeMapMany = employeeMapMany;
        }

        public Shift MapToEntity(Data.Models.Shift model)
        {
            return new Shift
            {
                Institution = _context.Institutions.Single(i => i.Id == model.Institution.Id),
                Start = model.Start,
                End = model.End,

                // Manual lookup
                Employees = model.Employees.Select(e => _context.Employees.Single(em => em.Id == e.Id)).ToList(),
            };
        }

        public Data.Models.Shift MapToModel(Shift entity)
        {
            return new Data.Models.Shift
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                
                Institution = _institutionMapper.MapToModel(entity.Institution),
                CheckIns = _checkInMapMany.Map(entity.CheckIns),
                Employees = _employeeMapMany.Map(entity.Employees)
            };
        }
    }
}
