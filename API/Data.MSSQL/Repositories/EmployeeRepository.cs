using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using System.Data.Entity;
using Data.Exceptions;
using System.Data;
using Data.Token;

namespace Data.MSSQL.Repositories
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
            if (_context.Employees.Any(x => x.Email == employee.Email && x.Organization.Id == employee.Organization.Id)) throw new ForbiddenException("An employee already exist with the given email");

            _context.Employees.Add(employee);
            return _context.SaveChanges() > 0 ? employee : null;
        }

        public IEnumerable<Employee> CreateMany(IEnumerable<Employee> employees)
        {
            var employeeDict = _context.Employees.ToDictionary(e => e.Email, e => e);
            var existingEmployees = employees.Where(e => employeeDict.ContainsKey(e.Email)).Select(e => e.Email);

            if(existingEmployees.Any()) throw new ForbiddenException($"The following emails do already exist: {String.Join(", ", existingEmployees)}");

            _context.Employees.AddRange(employees);
            return _context.SaveChanges() > 0 ? employees : null;
        }

        public void Delete(int id, int organizationId)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.Id == id && x.Organization.Id == organizationId);
            if (employee == null) throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
            
            employee.Shifts.Clear();
            employee.Preferences.Clear();
            employee.CheckIns.Clear();
            employee.Friendships.Clear();
            employee.Roles.Clear();
            employee.EmployeeAssignments.Clear();
            employee.Tokens.Clear();

            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public IEnumerable<Employee> ReadFromOrganization(int organizationId)
        {
            return _context.Employees
                .Where(e => e.Organization.Id == organizationId).OrderBy(x => x.Id);
        }

        public IEnumerable<Employee> ReadFromOrganization(string shortKey)
        {
            return _context.Employees
                .Where(e => e.Organization.ShortKey == shortKey);
        }

        public Employee Read(int id, int organizationId)
        {
            return _context.Employees
                .FirstOrDefault(employee => employee.Id == id && employee.Organization.Id == organizationId);
        }

        public Employee Read(int id, string shortKey)
        {
            return _context.Employees
                .FirstOrDefault(employee => employee.Id == id && employee.Organization.ShortKey == shortKey);
        }

        public int Update(Employee employee)
        {
            if(_context.Employees.Any(e => e.Email == employee.Email && e.Organization.Id == employee.Organization.Id && e.Id != employee.Id)) throw new ForbiddenException("An employee already exist with the given email");

            var dbEmployee = _context.Employees.Single(e => e.Id == employee.Id);

            dbEmployee.Email = employee.Email;
            dbEmployee.FirstName = employee.FirstName;
            dbEmployee.LastName = employee.LastName;
            dbEmployee.EmployeeTitle = _context.EmployeeTitles.Single(et => et.Id == employee.EmployeeTitle.Id);

            return _context.SaveChanges();
        }

        public Employee Read(string token)
        {
            return _context.Employees
                .FirstOrDefault(employee => employee.Tokens.Any(t => t.TokenHash == token));
        }

        public Employee Login(string email, string password)
        {
            var employee = _context.Employees.FirstOrDefault(m => m.Email == email);

            if (employee != null)
            {
                var hashPassword = HashManager.Hash(password + employee.Salt);
                if (employee.Password.Equals(hashPassword))
                {
                    var token = new Models.Token(TokenManager.GenerateLoginToken());
                    employee.Tokens.Add(token);
                    _context.SaveChanges();
                    return employee;
                }
            }
            throw new UnauthorizedAccessException("You entered an incorrect username or password!");
        }

        public void DeleteFriendship(Friendship friendship)
        {
            _context.Friendships.Remove(friendship);
            _context.SaveChanges();
        }

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}