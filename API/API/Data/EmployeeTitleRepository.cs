using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;

namespace API.Data
{
    public class EmployeeTitleRepository : IEmployeeTitleRepository, IDisposable
    {
        private IShiftPlannerDataContext _context;

        public EmployeeTitleRepository()
        {
            _context = new ShiftPlannerDataContext();
        }

        public EmployeeTitleRepository(ShiftPlannerDataContext context)
        {
            _context = context;
        }

        public EmployeeTitle Create(EmployeeTitle employeeTitle)
        {
            _context.EmployeeTitles.Add(employeeTitle);
            return _context.SaveChanges() > 0 ? employeeTitle : null;
        }

        public void Delete(int id, int institutionId)
        {
            var employeeTitle = _context.EmployeeTitles.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
            if (employeeTitle != null)
            {
                _context.EmployeeTitles.Remove(employeeTitle);
                _context.SaveChanges();
            }
        }

        public List<EmployeeTitle> ReadFromInstitution(int institutionId)
        {
            return _context.EmployeeTitles.Where(x => x.Institution.Id == institutionId).ToList();
        }

        public EmployeeTitle Read(int id, int institutionId)
        {
            return _context.EmployeeTitles.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
        }

        public int Update(EmployeeTitle employeeTitle)
        {
            _context.EmployeeTitles.Attach(employeeTitle);
            _context.MarkAsModified(employeeTitle);
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
