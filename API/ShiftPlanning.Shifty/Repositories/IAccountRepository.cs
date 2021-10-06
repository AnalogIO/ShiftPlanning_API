using System.Threading.Tasks;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.Shifty.Repositories
{
    public interface IAccountRepository
    {
        Task<EmployeeLoginResponse> Login(EmployeeLoginDTO loginDto);
    }
}