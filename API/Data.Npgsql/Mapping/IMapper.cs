namespace Data.Npgsql.Mapping
{
    public interface IMapper<TModel, TEntity>
    {
        TEntity MapToEntity(TModel model);
        TModel MapToModel(TEntity entity);
    }
}
