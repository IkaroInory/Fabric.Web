using System.Reflection;
using Fabric.Web.Configuration;
using Fabric.Web.Formatter;
using Fabric.Web.Repository;
using Fabric.Web.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Web;

public static class FabricWebConfiguration
{
    private static void AddContext(this IServiceCollection service, DatabaseConfiguration databaseConfiguration)
    {
        service.AddDbContext<Context>(options =>
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

    private static void AddIocComponent(this IServiceCollection service)
    {
        var attributes = new[] { typeof(ServiceAttribute), typeof(RepositoryAttribute) };

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes().Where(type => attributes.Any(type.IsDefined)));

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (
                    !@interface.GetInterfaces().Contains(typeof(IOriginalRepository)) &&
                    !@interface.GetInterfaces().Contains(typeof(IOriginalService))
                ) continue;

                service.AddTransient(@interface, type);
            }
        }
    }

    public static void AddFabricWebService(this IServiceCollection service, DatabaseConfiguration databaseConfiguration, FormatterType[] formatterTypes)
    {
        service.AddContext(databaseConfiguration);
        service.AddIocComponent();
        foreach (var formatterType in formatterTypes)
        {
            service.AddFormatter(formatterType);
        }
    }
}
