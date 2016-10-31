using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public interface IShiftRepository
    {
        Shift Create(Shift shift);
        List<Shift> Create(List<Shift> shifts);
        List<Shift> ReadFromInstitution(int institutionId);
        Shift Read(int id, int institutionId);
        int Update(Shift shift);
        void Delete(int id, int institutionId);
    }
}
