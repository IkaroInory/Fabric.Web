using Microsoft.EntityFrameworkCore;

namespace Fabric.Web.Configuration;

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

    internal string GetConnectionString() => $"Server={Server};" +
                                             $"Port={Port};" +
                                             $"Username={Username};" +
                                             $"Password={Password};" +
                                             $"Database={Database}";

    internal ServerVersion GetServerVersion() => ServerVersion.Parse(DatabaseVersion);
}
