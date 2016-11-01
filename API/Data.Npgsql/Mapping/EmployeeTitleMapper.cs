using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class EmployeeTitleMapper : IMapper<Data.Models.EmployeeTitle, EmployeeTitle>
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Data.Models.Institution, Institution> _institutionMapper;

        public EmployeeTitleMapper(IShiftPlannerDataContext context,
            IMapper<Data.Models.Institution, Institution> institutionMapper)
        {
            _context = context;
            _institutionMapper = institutionMapper;
        }

        public EmployeeTitle MapToEntity(Data.Models.EmployeeTitle model)
        {
            return new EmployeeTitle
            {
                Title = model.Title,

                // Manual lookup
                Institution = _context.Institutions.Single(i => i.Id == model.Institution.Id)
            };
        }

        public Data.Models.EmployeeTitle MapToModel(EmployeeTitle entity)
        {
            return new Data.Models.EmployeeTitle
            {
                Id = entity.Id,
                Title = entity.Title,
                Institution = _institutionMapper.MapToModel(entity.Institution)
            };
        }
    }
}
