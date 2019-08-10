using Data.Models;

namespace Data.Services
{
    public interface IEmailService
    {
        void SendNewAccountEmail(string fullname, string email, string password, Organization organization);
        void SendNewPasswordEmail(string fullname, string email, string password, Organization organization);
    }
}
