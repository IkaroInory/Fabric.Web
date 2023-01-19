namespace Fabric.Web.Repository;

public interface IBaseRepository<TEntity> : ICrud<TEntity>, IOriginalRepository where TEntity : class { }
