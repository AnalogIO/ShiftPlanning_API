using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Services
{
    public class EmployeeService
    {
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IInstitutionRepository institutionRepository, IEmployeeRepository employeeRepository)
        {
            _institutionRepository = institutionRepository;
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<Employee> GetEmployees(Manager manager)
        {
            return _employeeRepository.ReadFromInstitution(manager.Institution.Id);
        }
    }
}