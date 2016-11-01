using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class EmployeeMapper : IMapper<Data.Models.Employee, Employee>
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Data.Models.Institution, Institution> _institutionMapper;
        private readonly IMapper<Data.Models.EmployeeTitle, EmployeeTitle> _employeeTitleMapper;

        public EmployeeMapper(IShiftPlannerDataContext context,
            IMapper<Data.Models.Institution, Institution> institutionMapper,
            IMapper<Data.Models.EmployeeTitle, EmployeeTitle> employeeTitleMapper)
        {
            _context = context;
            _institutionMapper = institutionMapper;
            _employeeTitleMapper = employeeTitleMapper;
        }

        public Employee MapToEntity(Data.Models.Employee model)
        {
            return new Employee
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,

                // Manual lookup in database.
                Institution = _context.Institutions.Single(i => i.Id == model.Institution.Id),
                EmployeeTitle = _context.EmployeeTitles.Single(et => et.Id == model.EmployeeTitle.Id && et.Institution.Id == model.Institution.Id)
            };
        }

        public Data.Models.Employee MapToModel(Employee entity)
        {
            return new Data.Models.Employee
            {
                Id = entity.Id,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Institution = _institutionMapper.MapToModel(entity.Institution),
                EmployeeTitle = _employeeTitleMapper.MapToModel(entity.EmployeeTitle)
            };
        }
    }
}