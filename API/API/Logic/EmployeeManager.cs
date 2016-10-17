using API.Models;
using API.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Logic
{
    public class EmployeeManager
    {
        public static Employee UpdateEmployeeFromEmployeeDTO(Employee employee, UpdateEmployeeDTO employeeDto)
        {
            if (employeeDto.Email.Length > 0) employee.Email = employeeDto.Email;
            if (employeeDto.FirstName.Length > 0) employee.FirstName = employeeDto.FirstName;
            if (employeeDto.LastName.Length > 0) employee.LastName = employeeDto.LastName;
            return employee;
        }
    }
}