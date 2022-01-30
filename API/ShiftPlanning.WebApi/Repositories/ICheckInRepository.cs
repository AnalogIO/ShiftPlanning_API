using ShiftPlanning.Model.Models;

namespace ShiftPlanning.WebApi.Repositories
{
    public interface ICheckInRepository
    {
        void Delete(CheckIn checkin);
    }
}
