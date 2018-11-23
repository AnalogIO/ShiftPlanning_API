using System.Collections.Generic;
using Data.Models;
using Data.Repositories;
using DataTransferObjects.Employee;
using System.Linq;
using System;
using System.Data;
using System.IdentityModel;
using Data.Exceptions;
using Data.Token;

namespace Data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeTitleRepository _employeeTitleRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public EmployeeService(IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository, IEmployeeTitleRepository employeeTitleRepository, IScheduleRepository scheduleRepository)
        {
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _employeeTitleRepository = employeeTitleRepository;
            _scheduleRepository = scheduleRepository;
        }

        public IEnumerable<Employee> GetEmployees(int organizationId)
        {
            return _employeeRepository.ReadFromOrganization(organizationId);
        }

        public IEnumerable<Employee> GetEmployees(string shortKey)
        {
            return _employeeRepository.ReadFromOrganization(shortKey);
        }

        public Employee GetEmployee(int id, int institutionId)
        {
            return _employeeRepository.Read(id, institutionId);
        }

        public Employee GetEmployee(int id, string shortKey)
        {
            return _employeeRepository.Read(id, shortKey);
        }

        public Employee CreateEmployee(CreateEmployeeDTO employeeDto, Employee employee, Photo photo)
        {
            var newEmployee = new Employee
            {
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Organization = employee.Organization,
                Active = true,
                PhotoUrl = "",
                CheckIns = new List<CheckIn>(),
                Roles = new List<Role>(),
                Friendships = new List<Friendship>()
            };

            var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, employee.Organization.Id);
            if (title == null) throw new ObjectNotFoundException("Could not find a title corresponding to the given id");
            newEmployee.EmployeeTitle = title;
            var role = _employeeRepository.GetRoles().FirstOrDefault(r => r.Name == "Employee");
            newEmployee.Roles.Add(role);

            var pwgen = new PasswordGenerator();
            var emailService = new EmailService();

            var pw = pwgen.Next();
            //var hashedPw = HashManager.Hash(pw);
            var salt = HashManager.GenerateSalt();
            var saltedPw = HashManager.Hash(pw + salt);

            newEmployee.Salt = salt;
            newEmployee.Password = saltedPw;

            var createdEmployee = _employeeRepository.Create(newEmployee);
            if (createdEmployee == null) throw new BadRequestException("Please check your input again - the employee could not be created!");
            emailService.SendNewAccountEmail($"{createdEmployee.FirstName} {createdEmployee.LastName}", createdEmployee.Email, pw, employee.Organization);
            return createdEmployee;
        }

        public void ResetPassword(int id, int organizationId)
        {
            var employee = _employeeRepository.Read(id, organizationId);
            if (employee == null) throw new ObjectNotFoundException("Could not find the given employee!");

            var role = _employeeRepository.GetRoles().FirstOrDefault(r => r.Name == "Employee");

            if(!employee.Roles.Contains(role))
            {
                employee.Roles.Add(role);
            }

            var pwgen = new PasswordGenerator();
            var emailService = new EmailService();

            var pw = pwgen.Next();
            //var hashedPw = HashManager.Hash(pw);
            var salt = HashManager.GenerateSalt();
            var saltedPw = HashManager.Hash(pw + salt);

            employee.Salt = salt;
            employee.Password = saltedPw;

            _employeeRepository.Update(employee);
            emailService.SendNewPasswordEmail($"{employee.FirstName} {employee.LastName}", employee.Email, pw, employee.Organization);
        }

        public Employee UpdateEmployee(UpdateEmployeeDTO employeeDto, Employee updateEmployee, Photo photo)
        {
            //var updateEmployee = _employeeRepository.Read(employeeId, employee.Organization.Id);
            if (updateEmployee == null) throw new ObjectNotFoundException("Could not find an employee corresponding to the given id");
            
            if(!string.IsNullOrWhiteSpace(employeeDto.OldPassword) && !string.IsNullOrWhiteSpace(employeeDto.NewPassword))
            {
                if (employeeDto.NewPassword.Length < 8) throw new BadRequestException("Please specify a password with a minimum length of 8 characters");
                if (Login(updateEmployee.Email, employeeDto.OldPassword) != null)
                {
                    var salt = HashManager.GenerateSalt();
                    var hashedPw = HashManager.Hash(employeeDto.NewPassword + salt);
                    updateEmployee.Salt = salt;
                    updateEmployee.Password = hashedPw;
                }
            }

            updateEmployee.Email = employeeDto.Email;
            updateEmployee.FirstName = employeeDto.FirstName;
            updateEmployee.LastName = employeeDto.LastName;
            updateEmployee.Active = employeeDto.Active;
            updateEmployee.WantShifts = employeeDto.WantShifts;

            var title = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, updateEmployee.Organization.Id);
            if (title != null) updateEmployee.EmployeeTitle = title;
            _employeeRepository.Update(updateEmployee);
            return updateEmployee;
        }

        public IEnumerable<Employee> CreateManyEmployees(CreateEmployeeDTO[] employeeDtos, Employee employee)
        {
            return _employeeRepository.CreateMany(employeeDtos.Select(employeeDto => new Employee {
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Organization = employee.Organization,
                Active = true,
                EmployeeTitle = _employeeTitleRepository.Read(employeeDto.EmployeeTitleId, employee.Organization.Id) }));
        }

        public void DeleteEmployee(int employeeId, Employee employee)
        {
            _employeeRepository.Delete(employeeId, employee.Organization.Id);
        }

        public Employee Login(string email, string password)
        {
            return _employeeRepository.Login(email, password);
        }

        public Friendship CreateFriendship(Employee employee, int friendId)
        {
            if(employee.Friendships.Any(f => f.Friend_Id == friendId)) throw new BadRequestException("A friendship already exist!");

            var friend = _employeeRepository.Read(friendId, employee.Organization.Id);
            if (friend == null) throw new ObjectNotFoundException("The given id of the friend does not exist!");

            var friendship = new Friendship { Friend_Id = friend.Id };
            employee.Friendships.Add(friendship);

            _employeeRepository.Update(employee);
            return friendship;
        }

        public void DeleteFriendship(Employee employee, int friendId)
        {
            var friendship = employee.Friendships.SingleOrDefault(f => f.Friend_Id == friendId);
            _employeeRepository.DeleteFriendship(friendship);
        }



    }
}