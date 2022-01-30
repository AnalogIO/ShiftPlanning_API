using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Employee;
using ShiftPlanning.Shifty.Exceptions;

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

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EmployeeLoginResponse>();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            else
            {
                await Console.Error.WriteLineAsync($"Error calling API. Response {response.StatusCode} {await response.Content.ReadAsStringAsync()}");
                throw new ApiException();
            }
            
        }
    }
}