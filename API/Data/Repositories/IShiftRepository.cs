using System.Collections.Generic;
using Data.Models;
using System;

namespace Data.Repositories
{
    public interface IShiftRepository
    {
        Shift Create(Shift shift);
        IEnumerable<Shift> Create(IEnumerable<Shift> shifts);
        IEnumerable<Shift> ReadFromInstitution(int institutionId);
        Shift Read(int id, int institutionId);
        int Update(Shift shift);
        void Delete(int id, int institutionId);
        void Delete(IEnumerable<Shift> shifts);
        IEnumerable<Shift> GetOngoingShifts(int institutionId, DateTime currentTime);
    }
}
