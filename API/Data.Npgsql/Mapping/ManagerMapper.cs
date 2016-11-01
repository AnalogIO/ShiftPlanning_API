using System.Linq;
using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class ManagerMapper : IMapper<Data.Models.Manager, Manager>
    {
        private readonly IShiftPlannerDataContext _context;
        private readonly IMapper<Data.Models.Institution, Institution> _institutionMapper;
        private readonly IMapMany<Models.Token, Data.Models.Token> _tokenMapMany;

        public ManagerMapper(IShiftPlannerDataContext context,
            IMapper<Data.Models.Institution, Institution> institutionMapper, 
            IMapMany<Models.Token, Data.Models.Token> tokenMapMany)
        {
            _context = context;
            _institutionMapper = institutionMapper;
            _tokenMapMany = tokenMapMany;
        }

        public Manager MapToEntity(Data.Models.Manager model)
        {
            return new Manager
            {
                Institution = _context.Institutions.Single(i => i.Id == model.Institution.Id),
                Username = model.Username,
                Password = model.Password,
                Salt = model.Salt,
                Tokens = model.Tokens.Select(t => new Models.Token(t.TokenHash)
                {
                    Id = t.Id
                }).ToList()
            };
        }

        public Data.Models.Manager MapToModel(Manager entity)
        {
            return new Data.Models.Manager
            {
                Id = entity.Id,
                Username = entity.Username,
                Tokens = _tokenMapMany.Map(entity.Tokens),
                Institution = _institutionMapper.MapToModel(entity.Institution)
            };
        }
    }
}
