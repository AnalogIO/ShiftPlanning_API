using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Models;
using System.Data.Entity;

namespace Data.MSSQL.Repositories
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
