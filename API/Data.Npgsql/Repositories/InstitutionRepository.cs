using System.Linq;
using Data.Repositories;
using Data.Models;
using Data.Npgsql.Mapping;
using PGInstitution = Data.Npgsql.Models.Institution;

namespace Data.Npgsql.Repositories
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Institution, PGInstitution> _institutionMapper;

        public InstitutionRepository(IShiftPlannerDataContext context, IMapper<Institution, PGInstitution> institutionMapper)
        {
            _context = context;
            _institutionMapper = institutionMapper;
        }

        public bool HasApiKey(string apiKey)
        {
            return _context.Institutions.Any(i => i.ApiKey == apiKey);
        }

        public Institution Read(int id)
        {
            return _institutionMapper.MapToModel(_context.Institutions.Where(x => x.Id == id).FirstOrDefault());
        }

        public Institution Read(string apiKey)
        {
            var institution = _context.Institutions.Where(x => x.ApiKey == apiKey).FirstOrDefault();
            if(institution != null)
            {
                return _institutionMapper.MapToModel(institution);
            }
            return null;
        }
    }
}
