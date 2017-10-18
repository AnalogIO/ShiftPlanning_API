using System.Collections.Generic;
using Data.Models;
using System;

namespace Data.Repositories
{
    public interface IShiftRepository
    {
        Shift Create(Shift shift);
        IEnumerable<Shift> Create(IEnumerable<Shift> shifts);
        IEnumerable<Shift> ReadFromOrganization(int organizationId);
        IEnumerable<Shift> ReadFromOrganization(string organizationShortKey);
        Shift Read(int id, int organizationId);
        int Update(Shift shift);
        void Delete(int id, int organizationId);
        void Delete(IEnumerable<Shift> shifts);
        IEnumerable<Shift> GetOngoingShifts(int organizationId, DateTime currentTime);
        bool IsOrganisationOpen(string shortKey);
    }
}
