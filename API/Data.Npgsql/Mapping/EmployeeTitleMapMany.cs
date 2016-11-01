using System.Collections.Generic;
using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class EmployeeTitleMapMany : IMapMany<EmployeeTitle, Data.Models.EmployeeTitle>
    {
        public IEnumerable<Data.Models.EmployeeTitle> Map(IEnumerable<EmployeeTitle> source)
        {
            return source.Select(et => new Data.Models.EmployeeTitle
            {
                Id = et.Id,
                Title = et.Title,
                Institution = new Data.Models.Institution
                {
                    Id = et.Institution.Id,
                    Name = et.Institution.Name,
                    ApiKey = et.Institution.ApiKey
                }
            });
        }
    }
}
