using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Shift;
using System.Linq;

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
        
        public async Task<bool> CheckOut(int shiftId, int employeeId)
        {
            Console.WriteLine(employeeId);
            
            var response = await _client.PostAsync(ControllerUri + "/" + + shiftId + "/checkout?employeeId=" + employeeId, new StringContent(""));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckIn(int shiftId, int employeeId)
        {
            Console.WriteLine(employeeId);
            
            var response = await _client.PostAsync(ControllerUri + "/" + + shiftId + "/checkin?employeeId=" + employeeId, new StringContent(""));
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ShiftDTO>> TodayShifts()
        {
            var shifts = await _client.GetFromJsonAsync<IEnumerable<ShiftDTO>>(ControllerUri + "/today");

            if (shifts == null) return Enumerable.Empty<ShiftDTO>();
            
            return shifts.OrderBy(s => s.Start);
        }
    }
}