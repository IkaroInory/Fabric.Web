using Microsoft.EntityFrameworkCore;

namespace Fabric.Web.Configuration;

/// <summary>
/// Database connection configuration.
/// </summary>
public class DatabaseConfiguration
{
    public DatabaseType DatabaseType { get; init; }
    public string Server { get; init; }
    public int Port { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string Database { get; init; }
    public string DatabaseVersion { get; init; }

    public DatabaseConfiguration() : this(DatabaseType.Unknown, "", 0, "", "", "", "") { }

    private DatabaseConfiguration(DatabaseType databaseType, string server, int port, string username, string password, string database, string databaseVersion)
    {
        DatabaseType = databaseType;
        Server = server;
        Port = port;
        Username = username;
        Password = password;
        Database = database;
        DatabaseVersion = databaseVersion;
    }

    /// <summary>
    /// Get database connection string.
    /// </summary>
    /// <returns>Database connection string.</returns>
    internal string GetConnectionString() => $"Server={Server};" +
                                             $"Port={Port};" +
                                             $"Username={Username};" +
                                             $"Password={Password};" +
                                             $"Database={Database}";

    /// <summary>
    /// Get database server version. The method is only needed in specific cases, such as when the database is MySQL.
    /// </summary>
    /// <returns>A database server version of type ServerVersion.</returns>
    internal ServerVersion GetServerVersion() => ServerVersion.Parse(DatabaseVersion);
}
