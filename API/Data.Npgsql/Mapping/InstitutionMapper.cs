using Data.Npgsql.Models;

namespace Data.Npgsql.Mapping
{
    public class InstitutionMapper : IMapper<Data.Models.Institution, Institution>
    {
        public Institution MapToEntity(Data.Models.Institution model)
        {
            return new Institution
            {
                ApiKey = model.ApiKey,
                Name = model.Name
            };
        }

        public Data.Models.Institution MapToModel(Institution entity)
        {
            return new Data.Models.Institution
            {
                Id = entity.Id,
                Name = entity.Name,
                ApiKey = entity.ApiKey // TODO: Should this ever leave the database in this model?
            };
        }
    }
}
