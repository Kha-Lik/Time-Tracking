namespace TimeTracking.Common.Mappers
{

    public interface IEntityMapper<TEntity, TModel>
        where TEntity : class, new()
        where TModel : class, new()
    {
        TEntity MapToEntity(TModel model);
    }

    public interface IModelMapper<TEntity, TModel>
        where TEntity : class, new()
        where TModel : class, new()
    {
        TModel MapToModel(TEntity entity);
    }

    public interface IBaseMapper<TEntity, TModel> : IModelMapper<TEntity, TModel>, IEntityMapper<TEntity, TModel>
        where TEntity : class, new()
        where TModel : class, new()
    {
    }
}