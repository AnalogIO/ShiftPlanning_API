using System.Collections.Generic;
using System.Linq;

namespace Data.Npgsql.Mapping
{
    public class TokenMapMany : IMapMany<Models.Token, Data.Models.Token>
    {
        public IEnumerable<Data.Models.Token> Map(IEnumerable<Models.Token> source)
        {
            return source.Select(t => new Data.Models.Token(t.TokenHash)
            {
                Id = t.Id
            });
        }
    }
}
