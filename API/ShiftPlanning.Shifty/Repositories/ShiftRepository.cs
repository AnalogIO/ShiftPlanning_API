using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Shift;

namespace ShiftPlanning.Shifty.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly HttpClient _client;
        private const string ControllerUri = "api/shifts";

        public ShiftRepository(HttpClient client)
        {
            _client = client;
        }
        
        public Task<bool> CheckOut(int shiftId, int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CheckIn(int shiftId, int employeeId)
        {
            Console.WriteLine(employeeId);
            
            var values = new Dictionary<string, string>();
            values.Add("employeeId", employeeId.ToString());
            var content = new FormUrlEncodedContent(values);
            
            var response = await _client.PostAsync(ControllerUri + "/" + + shiftId + "/checkin?employeeId=" + employeeId, new StringContent(""));
            return await response.Content.ReadFromJsonAsync<CheckInDTO>() != null;
        }

        public Task<IEnumerable<ShiftDTO>> TodayShifts()
        {
            return _client.GetFromJsonAsync<IEnumerable<ShiftDTO>>(ControllerUri + "/today");
        }
    }
}