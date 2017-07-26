using System.Collections.Generic;
using Data.Models;
using DataTransferObjects.EmployeeTitles;
using System.Net.Mail;

namespace Data.Services
{
    public interface IEmailService
    {
        void SendNewAccountEmail(string fullname, string email, string password, Organization organization);
        void SendNewPasswordEmail(string fullname, string email, string password, Organization organization);
    }
}
