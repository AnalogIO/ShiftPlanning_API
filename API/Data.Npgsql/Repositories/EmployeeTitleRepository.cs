using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using Data.Exceptions;
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

        public void Delete(int id, int organizationId)
        {
            var employeeTitle = _context.EmployeeTitles.FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
            if (employeeTitle == null) throw new ObjectNotFoundException("The employee title could not be found");

            _context.EmployeeTitles.Remove(employeeTitle);
            _context.SaveChanges();
        }

        public IEnumerable<EmployeeTitle> ReadFromOrganization(int organizationId)
        {
            return _context.EmployeeTitles.Where(x => x.Organization.Id == organizationId);
        }

        public EmployeeTitle Read(int id, int organizationId)
        {
            return _context.EmployeeTitles.FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
        }

        public int Update(EmployeeTitle employeeTitle)
        {
            if(_context.EmployeeTitles.Any(et => et.Organization.Id == employeeTitle.Organization.Id && et.Title == employeeTitle.Title)) throw new ForbiddenException("An employee title with the given title does already exist");

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
