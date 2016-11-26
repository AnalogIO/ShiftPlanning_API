using System.Linq;
using Data.Repositories;
using Data.Models;

namespace Data.Npgsql.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IShiftPlannerDataContext _context;

        public OrganizationRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public bool Exists(string shortKey)
        {
            return _context.Organizations.Any(inst => inst.ShortKey == shortKey);
        }

        public bool Exists(int id)
        {
            return _context.Organizations.Any(inst => inst.Id == id);
        }

        public bool HasApiKey(string apiKey)
        {
            return _context.Organizations.Any(i => i.ApiKey == apiKey);
        }

        public Organization Read(int id)
        {
            return _context.Organizations.FirstOrDefault(x => x.Id == id);
        }

        public Organization Read(string apiKey)
        {
            return _context.Organizations.FirstOrDefault(x => x.ApiKey == apiKey);
        }
    }
}
