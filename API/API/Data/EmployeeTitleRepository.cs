using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;

namespace API.Data
{
    public class EmployeeTitleRepository : IEmployeeTitleRepository, IDisposable
    {
        private IShiftPlannerDataContext _context;

        public EmployeeTitleRepository()
        {
            _context = new ShiftPlannerDataContext();
        }

        public EmployeeTitleRepository(ShiftPlannerDataContext context)
        {
            _context = context;
        }

        public EmployeeTitle Create(CreateEmployeeTitleDTO employeeTitleDto)
        {
            var employeeTitle = new EmployeeTitle { Title = employeeTitleDto.Title };
            _context.EmployeeTitles.Add(employeeTitle);
            _context.SaveChanges();
            return employeeTitle;
        }

        public void Delete(int id)
        {
            var employeeTitle = _context.EmployeeTitles.Where(x => x.Id == id).FirstOrDefault();
            if (employeeTitle != null)
            {
                _context.EmployeeTitles.Remove(employeeTitle);
                _context.SaveChanges();
            }
        }

        public List<EmployeeTitle> Read()
        {
            return _context.EmployeeTitles.ToList();
        }

        public EmployeeTitle Read(int id)
        {
            return _context.EmployeeTitles.Where(x => x.Id == id).FirstOrDefault();
        }

        public int Update(EmployeeTitle employeeTitle)
        {
            _context.EmployeeTitles.Attach(employeeTitle);
            _context.MarkAsModified(employeeTitle);
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
