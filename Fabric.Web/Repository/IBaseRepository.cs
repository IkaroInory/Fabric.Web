namespace Fabric.Web.Repository;

public interface IBaseRepository<TEntity> : IOriginalRepository where TEntity : class
{
    int Insert(TEntity entity);
    int Delete(TEntity entity);
    int Update(TEntity entity);
    IEnumerable<TEntity> SelectAll();
    TEntity? SelectByPrimaryKey(params object[] values);
    IEnumerable<TEntity> SelectFromSql(FormattableString sql);
}
