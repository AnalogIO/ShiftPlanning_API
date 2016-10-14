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
        public Employee Create(RegisterDTO employeeDto)
        {
            var existingUser = _context.Employees.Where(x => x.Email == employeeDto.Email).FirstOrDefault();
            if (existingUser == null)
            {
                var employee = new Employee { Email = employeeDto.Email, FirstName = employeeDto.FirstName, LastName = employeeDto.LastName, Title = employeeDto.Title };
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return employee;
            }
            else
            {
                return null;
            }
        }

        public void Delete(int id)
        {
            var employee = _context.Employees.Where(x => x.Id == id).FirstOrDefault();
            if(employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        public List<Employee> Read()
        {
            return _context.Employees.ToList();
        }

        public Employee Read(int id)
        {
            return _context.Employees.Where(x => x.Id == id).FirstOrDefault();
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