using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Manager = Data.Npgsql.Models.Manager;

namespace Data.Npgsql.Mapping
{
    public class ManagerMapMany : IMapMany<Manager, Data.Models.Manager>
    {
        public IEnumerable<Data.Models.Manager> Map(IEnumerable<Manager> source)
        {
            return source.Select(m => new Data.Models.Manager
            {
                Id = m.Id,
                Username = m.Username,
                Tokens = m.Tokens.Select(t => new Data.Models.Token(t.TokenHash)
                {
                    Id = t.Id
                }),
                Institution = new Institution
                {
                    Id = m.Institution.Id,
                    Name = m.Institution.Name,
                    ApiKey = m.Institution.ApiKey
                }
            });
        }
    }
}
