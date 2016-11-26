using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Npgsql.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IShiftPlannerDataContext _context;

        public PhotoRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Photo Read(int photoId, int organizationId)
        {
            return _context.Photos.SingleOrDefault(photo => photo.Id == photoId && photo.Organization.Id == organizationId);
        }

        public Photo Read(int photoId, string organizationShortKey)
        {
            return _context.Photos.SingleOrDefault(photo => photo.Id == photoId && photo.Organization.ShortKey == organizationShortKey);
        }
    }
}
