using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using API.Models.DTO;

namespace API.Data
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {

        private IShiftPlannerDataContext _context;

        public EmployeeRepository()
        {
            _context = new ShiftPlannerDataContext();
        }

        public EmployeeRepository(ShiftPlannerDataContext context)
        {
            _context = context;
        }

        public Employee Create(Employee employee)
        {
            var existingEmployee = _context.Employees.Where(x => x.Email == employee.Email).FirstOrDefault();
            if (existingEmployee == null)
            {
                _context.Employees.Add(employee);
                return _context.SaveChanges() > 0 ? employee : null;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int id, int institutionId)
        {
            var employee = _context.Employees.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
            if(employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        public List<Employee> ReadFromInstitution(int institutionId)
        {
            return _context.Employees.Where(e => e.Institution.Id == institutionId).ToList();
        }

        public Employee Read(int id, int institutionId)
        {
            return _context.Employees.Where(x => x.Id == id && x.Institution.Id == institutionId).FirstOrDefault();
        }

        public int Update(Employee employee)
        {
            _context.Employees.Attach(employee);
            _context.MarkAsModified(employee);
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}