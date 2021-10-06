using System.Threading.Tasks;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.Shifty.Services
{
    public interface IAuthenticationService
    {
        bool IsValidLogin();
        Task<bool> LoginUser(EmployeeLoginDTO loginDto);
    }
}