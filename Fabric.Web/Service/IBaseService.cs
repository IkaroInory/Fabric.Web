using Fabric.Web.Repository;

namespace Fabric.Web.Service;

public interface IBaseService<TEntity> : ICrud<TEntity>, IOriginalService where TEntity : class { }
