using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Fabric.Web.Exception;
using Microsoft.EntityFrameworkCore;

namespace Fabric.Web.Repository;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly Context context;
    private readonly DbSet<TEntity> dbSet;

    protected BaseRepository(Context context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public int Insert(TEntity entity)
    {
        dbSet.Add(entity);
        return context.SaveChanges();
    }

    public int Delete(TEntity entity)
    {
        dbSet.Remove(entity);
        return context.SaveChanges();
    }

    public int Update(TEntity entity)
    {
        dbSet.Update(entity);
        return context.SaveChanges();
    }

    public IEnumerable<TEntity> SelectAll() => dbSet.ToList();

    public TEntity? SelectByPrimaryKey(params object[] values)
    {
        var tableName = typeof(TEntity).GetCustomAttribute<TableAttribute>()?.Name ?? typeof(TEntity).Name;

        var keys = (from property in typeof(TEntity).GetProperties()
            where property.GetCustomAttribute<KeyAttribute>() is not null
            select property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name).ToArray();

        if (keys.Length != values.Length) throw new RequiredPrimaryKeyParameterException();

        var condition = "";
        var index = 0;
        foreach (var key in keys)
        {
            if (index != 0) condition += " and ";
            if (values[index] is string)
            {
                condition += $"`{key}` = '{values[index]}'";
            }
            else
            {
                condition += $"`{key}` = {values[index]}";
            }

            index++;
        }

        var sql = $"select * from `{tableName}` where {condition}";

        var entities = this.dbSet.FromSqlRaw(sql).ToList();
        return entities.Count == 0 ? null : entities.First();
    }

    public IEnumerable<TEntity> SelectFromSql(FormattableString sql) => dbSet.FromSql(sql);
}
