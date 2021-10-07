using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Shifty.Authentication;
using ShiftPlanning.Shifty.Repositories;

namespace ShiftPlanning.Shifty.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAccountRepository _accountRepository;
        private readonly CustomAuthStateProvider _authStateProvider;
        
        public AuthenticationService(IAccountRepository accountRepository, CustomAuthStateProvider stateProvider, ILocalStorageService storageService)
        {
            _accountRepository = accountRepository;
            _authStateProvider = stateProvider;
            _localStorage = storageService;
        }

        public async Task<bool> LoginUser(EmployeeLoginDTO loginDto)
        {
            try
            {
                var login = await _accountRepository.Login(loginDto);
                await _localStorage.SetItemAsync("token", login.Token);
                return _authStateProvider.UpdateAuthState(login.Token);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}