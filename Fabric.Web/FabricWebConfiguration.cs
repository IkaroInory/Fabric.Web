using System.Reflection;
using Fabric.Web.Branch;
using Fabric.Web.Configuration;
using Fabric.Web.Formatter;
using Fabric.Web.Repository;
using Fabric.Web.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Web;

/// <summary>
/// Fabric.Web global configuration class.
/// </summary>
public static class FabricWebConfiguration
{
    /// <summary>
    /// Register a DbContext subclass as a scoped service in the ASP.NET Core application service provider.
    /// </summary>
    /// <param name="services">A collection of services for the application to compose. This is useful for adding user provided or framework provided services.</param>
    /// <param name="databaseConfiguration">A configuration of database. This is required to connect the database.</param>
    private static void AddContext(this IServiceCollection services, DatabaseConfiguration databaseConfiguration)
    {
        services.AddDbContext<Context>(options =>
        {
            switch (databaseConfiguration.DatabaseType)
            {
                case DatabaseType.SqlServer:
                    options.UseSqlServer(databaseConfiguration.GetConnectionString());
                    break;
                case DatabaseType.MySql:
                    options.UseMySql(databaseConfiguration.GetConnectionString(), databaseConfiguration.GetServerVersion());
                    break;
                case DatabaseType.MariaDB:
                    options.UseMySql(databaseConfiguration.GetConnectionString(), databaseConfiguration.GetServerVersion());
                    break;
                case DatabaseType.Unknown:
                    return;
                default:
                    return;
            }
        });
    }

    /// <summary>
    /// Add components into the IOC in the APS.NET Core application.
    /// </summary>
    /// <param name="services">A collection of services for the application to compose. This is useful for adding user provided or framework provided services.</param>
    private static void AddIocComponents(this IServiceCollection services)
    {
        var attributes = new[] { typeof(ServiceAttribute), typeof(RepositoryAttribute) };

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes().Where(type => attributes.Any(type.IsDefined)));

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!@interface.GetInterfaces().Contains(typeof(IOriginalRepository)) && !@interface.GetInterfaces().Contains(typeof(IOriginalService)))
                    continue;

                services.AddTransient(@interface, type);
            }
        }
    }

    /// <summary>
    /// Add Fabric.Web Service into the web application.
    /// </summary>
    /// <param name="services">A collection of services for the application to compose. This is useful for adding user provided or framework provided services.</param>
    /// <param name="databaseConfiguration">A configuration of database. This is required to connect the database.</param>
    public static void AddFabricWebService(this IServiceCollection services, DatabaseConfiguration databaseConfiguration)
    {
        services.AddFabricWebService(databaseConfiguration, ArraySegment<FormatterType>.Empty);
    }

    /// <summary>
    /// Add Fabric.Web Service into the web application.
    /// </summary>
    /// <param name="services">A collection of services for the application to compose. This is useful for adding user provided or framework provided services.</param>
    /// <param name="databaseConfiguration">A configuration of database. This is required to connect the database.</param>
    /// <param name="formatterTypes">A collection of formatter type you need.</param>
    public static void AddFabricWebService(this IServiceCollection services, DatabaseConfiguration databaseConfiguration, IEnumerable<FormatterType> formatterTypes)
    {
        PreLaunchCheck.CheckAll();

        services.AddContext(databaseConfiguration);
        services.AddIocComponents();
        foreach (var formatterType in formatterTypes)
        {
            services.AddFormatter(formatterType);
        }
    }
}
