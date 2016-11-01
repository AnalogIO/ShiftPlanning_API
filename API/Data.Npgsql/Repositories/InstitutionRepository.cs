using System.Linq;
using Data.Repositories;

namespace Data.Npgsql.Repositories
{
    public class InstitutionRepository : IInstitutionRepository
    {
        private readonly IShiftPlannerDataContext _context;

        public InstitutionRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public bool HasApiKey(string apiKey)
        {
            return _context.Institutions.Any(i => i.ApiKey == apiKey);
        }
    }
}
