using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Shift;

namespace ShiftPlanning.Shifty.Repositories
{
    public interface IShiftRepository
    {
        Task<bool> CheckOut(int shiftId, int employeeId);
        Task<bool> CheckIn(int shiftId, int employeeId);
        Task<IEnumerable<ShiftDTO>> TodayShifts();
    }
}