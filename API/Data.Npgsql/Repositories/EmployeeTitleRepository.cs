using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;

namespace Data.Npgsql.Repositories
{
    public class EmployeeTitleRepository : IEmployeeTitleRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        
        public EmployeeTitleRepository(IShiftPlannerDataContext context)
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
            var employeeTitle = _context.EmployeeTitles.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
            if (employeeTitle != null)
            {
                _context.EmployeeTitles.Remove(employeeTitle);
                _context.SaveChanges();
            }
        }

        public IEnumerable<EmployeeTitle> ReadFromInstitution(int institutionId)
        {
            return _context.EmployeeTitles.Where(x => x.Institution.Id == institutionId);
        }

        public EmployeeTitle Read(int id, int institutionId)
        {
            return _context.EmployeeTitles.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
        }

        public int Update(EmployeeTitle employeeTitle)
        {
            var dbEmployeeTitle = _context.EmployeeTitles.Single(et => et.Id == employeeTitle.Id);

            dbEmployeeTitle.Title = employeeTitle.Title;
            
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
