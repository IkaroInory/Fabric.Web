using System.Reflection;
using Fabric.Web.Entity;
using Microsoft.EntityFrameworkCore;

namespace Fabric.Web.Repository;

/// <summary>
/// EF Core context.
/// </summary>
public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add entities marked with Entity attribute to the EF Core model.

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetExportedTypes().Where(type => type.IsDefined(typeof(EntityAttribute))));

        foreach (var type in types)
        {
            modelBuilder.Entity(type);
        }

        base.OnModelCreating(modelBuilder);
    }
}
