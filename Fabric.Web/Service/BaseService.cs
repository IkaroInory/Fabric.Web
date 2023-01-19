using Fabric.Web.Repository;

namespace Fabric.Web.Service;

public class BaseService<TEntity, TRepository> : IBaseService<TEntity>
    where TEntity : class
    where TRepository : IBaseRepository<TEntity>
{
    protected TRepository Repository { get; }

    protected BaseService(TRepository repository) { Repository = repository; }

    public int Insert(TEntity entity) => Repository.Insert(entity);
    public int Delete(TEntity entity) => Repository.Delete(entity);
    public int Update(TEntity entity) => Repository.Update(entity);
    public IEnumerable<TEntity> SelectAll() => Repository.SelectAll();
    public TEntity? SelectByPrimaryKey(params object[] values) => Repository.SelectByPrimaryKey(values);
    public IEnumerable<TEntity> SelectFromSql(FormattableString sql) => Repository.SelectFromSql(sql);
}
