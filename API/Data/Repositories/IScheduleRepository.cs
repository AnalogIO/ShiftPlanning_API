using System.Collections.Generic;
using Data.Models;

namespace Data.Repositories
{
    public interface IScheduleRepository
    {
        Schedule Create(Schedule schedule);
        IEnumerable<Schedule> ReadFromInstitution(int institutionId);
        Schedule Read(int id, int institutionId);
        int Update(Schedule schedule);
        void Delete(int id, int institutionId);
    }
}
