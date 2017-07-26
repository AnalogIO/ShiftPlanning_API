using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.EmployeeTitles;
using System.Net.Mail;

namespace Data.Services
{
    public interface IEmailService
    {
        void SendNewPassword(string fullname, string email, string password, Organization organization);
    }
}
