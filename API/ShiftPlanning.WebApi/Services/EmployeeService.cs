using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using PodioAPI;
using PodioAPI.Models;
using ShiftPlanning.Common.Configuration;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Model.Models;
using ShiftPlanning.WebApi.Exceptions;
using ShiftPlanning.WebApi.Repositories;
using ShiftPlanning.WebApi.Token;

namespace ShiftPlanning.WebApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly FtpSettings _ftpSettings;
        private readonly PodioSettings _podioSettings;

        public EmployeeService(IOrganizationRepository organizationRepository, IEmployeeRepository employeeRepository, IScheduleRepository scheduleRepository, FtpSettings ftpSettings, PodioSettings podioSettings)
        {
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _scheduleRepository = scheduleRepository;
            _ftpSettings = ftpSettings;
            _podioSettings = podioSettings;
        }

        public IEnumerable<Employee> GetEmployees(int organizationId)
        {
            return _employeeRepository.ReadFromOrganization(organizationId);
        }

        public IEnumerable<Employee> GetEmployees(string shortKey)
        {
            return _employeeRepository.ReadFromOrganization(shortKey);
        }

        public IEnumerable<Employee> GetEmployeesByActivity(int organizationId, bool active = true)
        {
            return _employeeRepository.ReadFromOrganization(organizationId).Where(x => x.Active == active);
        }

        public IEnumerable<Employee> GetEmployeesByActivity(string shortKey, bool active = true)
        {
            return _employeeRepository.ReadFromOrganization(shortKey).Where(x => x.Active == active);
        }

        public Employee GetEmployee(int id, int institutionId)
        {
            return _employeeRepository.Read(id, institutionId);
        }

        public Employee GetEmployee(int id, string shortKey)
        {
            return _employeeRepository.Read(id, shortKey);
        }

        public Employee CreateEmployee(CreateEmployeeDTO employeeDto, Employee employee)
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
                Role_ = new List<Role>(),
                Friendships = new List<Friendship>()
            };

            var role = _employeeRepository.GetRoles().FirstOrDefault(r => r.Name == "Employee");
            newEmployee.Role_.Add(role);

            var pwgen = new PasswordGenerator.Password();
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

            if(!employee.Role_.Contains(role))
            {
                employee.Role_.Add(role);
            }

            var pwgen = new PasswordGenerator.Password();
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
                EmployeeTitle = employee.EmployeeTitle }));
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

        public Employee CreateEmployeeFromPodio(CreateEmployeeDTO employeeDto, Model.Models.Organization organization)
        {
            var newEmployee = new Employee
            {
                Email = employeeDto.Email,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Organization = organization,
                Active = false,
                PhotoUrl = "",
                CheckIns = new List<CheckIn>(),
                Role_ = new List<Role>(),
                Friendships = new List<Friendship>(),
                EmployeeTitle = employeeDto.EmployeeTitle
            };

            var role = _employeeRepository.GetRoles().FirstOrDefault(r => r.Name == "Employee");
            newEmployee.Role_.Add(role);

            var createdEmployee = _employeeRepository.Create(newEmployee);
            if (createdEmployee == null) throw new BadRequestException("Please check your input again - the employee could not be created!");
            return createdEmployee;
        }

        public int SyncEmployees(string shortKey)
        {
            var clientId = _podioSettings.PodioClientId;
            var clientSecret = _podioSettings.PodioClientSecret;

            var appId = _podioSettings.PodioAppId;
            var appToken = _podioSettings.PodioAppToken;

            var ftpHost = _ftpSettings.FtpHost;
            var ftpUsername = _ftpSettings.FtpUsername;
            var ftpPassword = _ftpSettings.FtpPassword;

            var imageFieldId = 30905042;
            var emailFieldId = 30905031;
            var activeFieldId = 176776679;

            var podio = new Podio(clientId, clientSecret);

            podio.AuthenticateWithApp(appId, appToken);

            var matchedEmployees = new List<Employee>();

            var filteredItems = podio.ItemService.FilterItems(appId: 3999665, sortBy: "last_edit_on", sortDesc: true, limit: 500);
            var employees = _employeeRepository.ReadFromOrganization(shortKey);

            foreach(var item in filteredItems.Items)
            {
                var index = item.Title.IndexOf(' ');
                if (index == -1) continue;
                var firstName = item.Title.Substring(0, index).Trim();
                var lastName = item.Title.Substring(index).Trim();

                var email = "";

                var emailItem = item.Fields.FirstOrDefault(x => x.FieldId.Equals(emailFieldId));
                if(emailItem != null)
                {
                    email = (string)emailItem.Values.First["value"];
                }

                var active = false;
                var activeItem = item.Fields.FirstOrDefault(x => x.FieldId.Equals(activeFieldId));
                if (activeItem != null)
                {
                    var status = GetFieldStringValue(activeItem, "status");
                    var text = GetFieldStringValue(activeItem, "text");
                    active = text.Equals("Yes");
                }

                var titles = new List<string>();
                var titleItem = item.Fields.FirstOrDefault(x => x.FieldId.Equals(30905027));
                if(titleItem != null)
                {
                    var id = titleItem.Values.Select(x => (int)x["value"]["id"]).ToList();

                    if(id.Any(x => x.Equals(12)))
                    {
                        titles.Add("Penguin");
                    }

                    if(id.Any(x => x.Equals(15)))
                    {
                        titles.Add("Board member");
                    }

                    if(id.Any(x => x.Equals(17)))
                    {
                        titles.Add("Kitchen Manager");
                    }

                    if(id.Any(x => x.Equals(18)))
                    {
                        titles.Add("Storage Manager");
                    }

                    if (id.Any(x => x.Equals(19)))
                    {
                        titles.Add("Auditor");
                    }

                    if (id.Any(x => x.Equals(20)))
                    {
                        titles.Add("Shiftplanner");
                    }

                    if (id.Any(x => x.Equals(14)))
                    {
                        titles.Add("Barista");
                    }

                }

                var employeeTitle = string.Join(", ", titles);

                var employee = employees.FirstOrDefault(x => (x.FirstName.ToLower().Equals(firstName.ToLower()) && x.LastName.ToLower().Equals(lastName.ToLower())) || x.PodioId == item.ItemId || x.Email.ToLower().Equals(email.ToLower()));
                if(employee == null)
                {
                    // ADVANCED SEARCH FOR EXISTING EMPLOYEES
                    /*
                    var separator = lastName.IndexOf('-');
                    if (separator == -1) separator = lastName.IndexOf(" ");
                    if (separator != -1)
                    {
                        var alternativeLastName = lastName.Substring(0, separator);
                        var alternativeEmployee = employees.FirstOrDefault(x => x.FirstName.ToLower().Equals(firstName.ToLower()) && x.LastName.ToLower().Contains(alternativeLastName.ToLower()));
                        if (alternativeEmployee == null)
                        {
                            alternativeLastName = lastName.Substring(separator + 1);
                            alternativeEmployee = employees.FirstOrDefault(x => x.FirstName.ToLower().Equals(firstName.ToLower()) && x.LastName.ToLower().Contains(alternativeLastName.ToLower()));
                        }

                        if (alternativeEmployee != null) employee = alternativeEmployee;
                    }

                    if(employee == null) //employee is unknown and needs to be created
                    {
                        var newEmployeeDto = new CreateEmployeeDTO { Email = email, FirstName = firstName, LastName = lastName };
                        var analogOrganization = _organizationRepository.ReadByShortKey("analog");
                        employee = CreateEmployeeFromPodio(newEmployeeDto, analogOrganization);
                    }*/


                    //create employee if they do not exist in database
                    var newEmployeeDto = new CreateEmployeeDTO { Email = email, FirstName = firstName, LastName = lastName, EmployeeTitle = employeeTitle };
                    var analogOrganization = _organizationRepository.ReadByShortKey(shortKey);
                    employee = CreateEmployeeFromPodio(newEmployeeDto, analogOrganization);
                }

                if (employee != null)
                {
                    if (string.IsNullOrEmpty(employee.PhotoUrl)) {
                        var pItem = item.Fields.FirstOrDefault(x => x.FieldId.Equals(imageFieldId));
                        if (pItem != null)
                        {
                            var photoUrl = GetFieldStringValue(pItem, "thumbnail_link");
                            var photoName = GetFieldStringValue(pItem, "name");
                            var fileId = GetFieldIntValue(pItem, "file_id");

                            var file = podio.FileService.GetFile(fileId);
                            var fileResponse = podio.FileService.DownloadFile(file);
                            var photoMemoryStream = new MemoryStream(fileResponse.FileContents);

                            var fileName = $"{item.ItemId}{photoName.Substring(photoName.LastIndexOf('.'))}";
                            FtpUpload(photoMemoryStream, ftpHost, ftpUsername, ftpPassword, fileName);

                            employee.PhotoUrl = $"http://img.cafeanalog.dk/baristas/{fileName}";
                        }
                    }
                    employee.PodioId = item.ItemId;
                    employee.Active = active;
                    employee.EmployeeTitle = employeeTitle;
                    matchedEmployees.Add(employee);
                }
            }

            return _employeeRepository.UpdateMany(matchedEmployees);
        }

        private static string GetFieldStringValue(ItemField item, string fieldKey)
        {
            return (string)item.Values.First["value"][fieldKey];
        }

        private static int GetFieldIntValue(ItemField item, string fieldKey)
        {
            return (int)item.Values.First["value"][fieldKey];
        }

        private static string FtpUpload(MemoryStream memStream, string to_uri, string user_name, string password, string fileName)
        {
            var request = (FtpWebRequest)WebRequest.Create(to_uri + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user_name, password);
            request.UseBinary = true;
            var buffer = new byte[memStream.Length];
            memStream.Read(buffer, 0, buffer.Length);
            memStream.Close();
            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(buffer, 0, buffer.Length);
            }
            return string.Empty;
        }

    }
}