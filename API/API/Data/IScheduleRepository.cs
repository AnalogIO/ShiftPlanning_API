using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IScheduleRepository
    {
        Schedule Create(Schedule schedule);
        List<Schedule> ReadFromInstitution(int institutionId);
        Schedule Read(int id, int institutionId);
        int Update(Schedule schedule);
        void Delete(int id, int institutionId);
    }
}
