using ShiftPlanning.Model;
using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
{
    public class CheckInRepository : ICheckInRepository
    {
        private readonly IShiftPlannerDataContext _context;

        public CheckInRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public void Delete(CheckIn checkin)
        {
            _context.CheckIns.Remove(checkin);
            _context.SaveChanges();
        }


    }
}
