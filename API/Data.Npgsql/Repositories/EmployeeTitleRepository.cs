using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Npgsql.Mapping;
using Data.Repositories;
using PGEmployeeTitle = Data.Npgsql.Models.EmployeeTitle;

namespace Data.Npgsql.Repositories
{
    public class EmployeeTitleRepository : IEmployeeTitleRepository, IDisposable
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<EmployeeTitle, PGEmployeeTitle> _employeeTitleMapper;
        private readonly IMapMany<PGEmployeeTitle, EmployeeTitle> _employeeTitleMapMany;
        
        public EmployeeTitleRepository(IShiftPlannerDataContext context, 
            IMapper<EmployeeTitle, PGEmployeeTitle> employeeTitleMapper,
            IMapMany<PGEmployeeTitle, EmployeeTitle> employeeTitleMapMany)
        {
            _context = context;
            _employeeTitleMapper = employeeTitleMapper;
            _employeeTitleMapMany = employeeTitleMapMany;
        }

        public EmployeeTitle Create(EmployeeTitle employeeTitle)
        {
            var newEmployeeTitle = _employeeTitleMapper.MapToEntity(employeeTitle);

            _context.EmployeeTitles.Add(newEmployeeTitle);
            return _context.SaveChanges() > 0 ? _employeeTitleMapper.MapToModel(newEmployeeTitle) : null;
        }

        public void Delete(int id, int institutionId)
        {
            var employeeTitle = _context.EmployeeTitles.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId);
            if (employeeTitle != null)
            {
                _context.EmployeeTitles.Remove(employeeTitle);
                _context.SaveChanges();
            }
        }

        public IEnumerable<EmployeeTitle> ReadFromInstitution(int institutionId)
        {
            return _employeeTitleMapMany.Map(_context.EmployeeTitles.Where(x => x.Institution.Id == institutionId));
        }

        public EmployeeTitle Read(int id, int institutionId)
        {
            return _employeeTitleMapper.MapToModel(_context.EmployeeTitles.FirstOrDefault(x => x.Id == id && x.Institution.Id == institutionId));
        }

        public int Update(EmployeeTitle employeeTitle)
        {
            var dbEmployeeTitle = _context.EmployeeTitles.Single(et => et.Id == employeeTitle.Id);

            dbEmployeeTitle.Title = employeeTitle.Title;
            
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
