using System.Threading.Tasks;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.Shifty.Services
{
    public interface IAuthenticationService
    {
        Task<bool> LoginUser(EmployeeLoginDTO loginDto);
    }
}