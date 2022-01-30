using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MudBlazor.Extensions;
using ShiftPlanning.DTOs.Employee;

namespace ShiftPlanning.Shifty.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly HttpClient _client;
        private const string ControllerUri = "api/account";

        public AccountRepository(HttpClient client)
        {
            _client = client;
        }
        
        public async Task<EmployeeLoginResponse> Login(EmployeeLoginDTO loginDto)
        {
            var response = await _client.PostAsJsonAsync(ControllerUri + "/login", loginDto);
            return await response.Content.ReadFromJsonAsync<EmployeeLoginResponse>();
        }
    }
}