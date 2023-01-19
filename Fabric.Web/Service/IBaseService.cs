namespace Fabric.Web.Service;

public interface IBaseService<TEntity> : IOriginalService where TEntity : class
{
    int Insert(TEntity entity);
    int Delete(TEntity entity);
    int Update(TEntity entity);
    IEnumerable<TEntity> SelectAll();
    TEntity? SelectByPrimaryKey(params object[] values);
    IEnumerable<TEntity> SelectFromSql(FormattableString sql);
}
