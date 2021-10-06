using System;
using System.Net.Http;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Shifty.Repositories;
using ShiftPlanning.Shifty.States;

namespace ShiftPlanning.Shifty.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly LoginState _loginState;
        
        public AuthenticationService(IAccountRepository accountRepository, LoginState state)
        {
            _accountRepository = accountRepository;
            _loginState = state;
        }
        
        public bool IsValidLogin()
        {
            return _loginState.UserLogin != null; //TODO improve
        }

        public async Task<bool> LoginUser(EmployeeLoginDTO loginDto)
        {
            try
            {
                _loginState.UserLogin = await _accountRepository.Login(loginDto);
                return true;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}