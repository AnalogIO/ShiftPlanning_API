using Data.Models;

namespace Data.Repositories
{
    public interface ICheckInRepository
    {
        void Delete(CheckIn checkin);
    }
}
