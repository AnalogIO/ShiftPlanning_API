using System.Linq;
using Data.Repositories;
using Data.Models;

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

        public Institution Read(int id)
        {
            return _context.Institutions.Where(x => x.Id == id).FirstOrDefault();
        }

        public Institution Read(string apiKey)
        {
            var institution = _context.Institutions.Where(x => x.ApiKey == apiKey).FirstOrDefault();
            if(institution != null)
            {
                return institution;
            }
            return null;
        }
    }
}
