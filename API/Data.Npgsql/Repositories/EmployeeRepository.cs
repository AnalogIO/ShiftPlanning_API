using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;

namespace Data.Npgsql.Repositories
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;

        public EmployeeRepository(IShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Employee Create(Employee employee)
        {
            if (!_context.Employees.Any(x => x.Email == employee.Email))
            {
                _context.Employees.Add(employee);
                return _context.SaveChanges() > 0 ? employee : null;
            }
            return null;
        }

        public void Delete(int id, int institutionId)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
            if(employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Employee> ReadFromInstitution(int institutionId)
        {
            return _context.Employees.Where(e => e.Institution.Id == institutionId).OrderBy(x => x.Id);
        }

        public Employee Read(int id, int institutionId)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
        }

        public int Update(Employee employee)
        {
            var dbEmployee = _context.Employees.Single(e => e.Id == employee.Id);

            dbEmployee.Email = employee.Email;
            dbEmployee.FirstName = employee.FirstName;
            dbEmployee.LastName = employee.LastName;
            dbEmployee.EmployeeTitle = _context.EmployeeTitles.Single(et => et.Id == employee.EmployeeTitle.Id);

            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}