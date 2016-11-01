using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Npgsql.Mapping;
using Data.Repositories;
using PGEmployee = Data.Npgsql.Models.Employee;

namespace Data.Npgsql.Repositories
{
    public class EmployeeRepository : IEmployeeRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Employee, PGEmployee> _employeeMapper;
        private readonly IMapMany<PGEmployee, Employee> _employeeMapMany;

        public EmployeeRepository(IShiftPlannerDataContext context, IMapper<Employee, PGEmployee> employeeMapper,
            IMapMany<PGEmployee, Employee> employeeMapMany)
        {
            _context = context;
            _employeeMapper = employeeMapper;
            _employeeMapMany = employeeMapMany;
        }

        public Employee Create(Employee employee)
        {
            if (!_context.Employees.Any(x => x.Email == employee.Email))
            {
                var newEmployee = _employeeMapper.MapToEntity(employee);

                _context.Employees.Add(newEmployee);
                return _context.SaveChanges() > 0 ? _employeeMapper.MapToModel(newEmployee) : null;
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
            return _employeeMapMany.Map(_context.Employees.Where(e => e.Institution.Id == institutionId));
        }

        public Employee Read(int id, int institutionId)
        {
            return _employeeMapper.MapToModel(_context.Employees.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId));
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