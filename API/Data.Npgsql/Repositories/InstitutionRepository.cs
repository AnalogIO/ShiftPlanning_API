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

        public bool Exists(string shortKey)
        {
            return _context.Institutions.Any(inst => inst.ShortKey == shortKey);
        }

        public bool Exists(int id)
        {
            return _context.Institutions.Any(inst => inst.Id == id);
        }

        public bool HasApiKey(string apiKey)
        {
            return _context.Institutions.Any(i => i.ApiKey == apiKey);
        }

        public Institution Read(int id)
        {
            return _context.Institutions.FirstOrDefault(x => x.Id == id);
        }

        public Institution Read(string apiKey)
        {
            return _context.Institutions.FirstOrDefault(x => x.ApiKey == apiKey);
        }
    }
}
