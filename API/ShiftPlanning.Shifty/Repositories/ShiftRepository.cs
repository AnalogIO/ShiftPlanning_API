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
        private const string ControllerUri = "/api/shifts";

        public ShiftRepository(HttpClient client)
        {
            _client = client;
        }
        
        public bool CheckOut(int shiftId, int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckIn(int shiftId, int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ShiftDTO>> TodayShifts()
        {
            return _client.GetFromJsonAsync<IEnumerable<ShiftDTO>>(ControllerUri + "/today");
        }
    }
}