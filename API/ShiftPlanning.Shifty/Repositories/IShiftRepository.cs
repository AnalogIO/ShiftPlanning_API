using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftPlanning.DTOs.Shift;

namespace ShiftPlanning.Shifty.Repositories
{
    public interface IShiftRepository
    {
        bool CheckOut(int shiftId, int employeeId);
        bool CheckIn(int shiftId, int employeeId);
        Task<IEnumerable<ShiftDTO>> TodayShifts();
    }
}