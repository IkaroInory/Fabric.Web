# Fabric.Web

Fabric.Web is a .NET library to help coding easily like Spring Boot + Mybatis with Java.

You can use Controller, Service and Repository with Fabric.Web build a Web API Solution.

## Description

- Integrate Microsoft Entity Framework Core(EF Core).
- Support SQL Server, MySQL and MariaDB.
- Integrate common CRUD.
- Support SQL query.
- Support UTC time formatting.

## Configuration

### Register Fabric.Web service.

```csharp
builder.Services.AddFabricWebService(new DatabaseConfiguration());
```

You can configure properties in the class ``DatabaseConfiguration`` like this:

```csharp
new DatabaseConfiguration
{
    DatabaseType    =   <DatabaseType>,
    Server          =   <Server URL>,
    Port            =   <Port>,
    Username        =   <Database Username>,
    Password        =   <Database Password>,
    Database        =   <Schema Name>,
    DatabaseVersion =   [Database Version]
}
```

The property ``DatabaseVersion`` is not required if you are connecting **SQL Server**.

### Mark IOC containers with attributes.

In Web API Solution, we often use Controller-Service-Repository(DAO) pattern,
therefore we use attributes for Inversion of Control these containers.

Generally, controllers is managed by ASP.NET Core(Because of MVC container),
so we only need to mark services and repositories.

- Mark entities with ``EntityAttribute``.

```csharp
[Entity]
public class Foo
{
    [Key]
    [Required]
    public required string Name { get; set; }

    [Required]
    [MinLength(32), MaxLength(32)]
    public required string Phone{ get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
```

- Mark repository with ``RepositoryAttribute``.

```csharp
public interface IFooRepository
{
    IEnumerable<Foo> GetFooList();
}

[Repository]
public class FooRepository : IFooRepository
{
    // Fabric.Web.Repository.Context
    private readonly Context context;

    // Auto-wire by constructor
    public FooRepository(Context context) { this.context = context; }

    public IEnumerable<Foo> GetFooList() => context.Set<Foo>().ToList();
}
```

- Mark service with ``ServiceAttribute``.

```csharp
public interface IFooService
{
    Foo GetFooByName(string name);
}

[Service]
public class FooService : IFooService
{
    private readonly IFooRepository fooRepository;

    // Auto-wire by constructor
    public FooService(IFooRepository fooRepository) { this.fooRepository = fooRepository; }

    public Foo GetFooByName(string name) => fooRepository.GetFooList().Where(foo => foo.Name == name).First();
}
```

### Wire services into the controller

.NET can auto-wire your containers by IOC,
you only need add a constructor into your controller.

```csharp
[ApiController]
[Route("[controller]")]
public class FooController : ControllerBase
{
    private readonly IFooService fooService;

    // Auto-wire by constructor
    public TestController(IFooService fooService) { this.fooService = fooService; }
}
```

## CRUD Interface

Fabric.Web provides a interface ``IBaseRepository<TEntity>`` includes some crud methods,
you can use the interface in repositories like this:

```csharp
public interface IFooRepository : IBaseRepository<Foo> { }

[Repository]
public class FooRepository : BaseRepository<Foo>, IFooRepository
{
    // Required.
    public FooRepository(Context context) : base(context) { }
}
```

You can use CRUD method after that:

```csharp
[Service]
public class FooService : IFooService
{
    private readonly IFooRepository fooRepository;

    public FooService(IFooRepository fooRepository) { this.fooRepository = fooRepository; }

    // IBaseRepository<TEntity>.SelectByPrimaryKey()
    public string? GetPhoneByName(string name) => SelectByPrimaryKey(name)?.Phone;
}
```

Similarly, services also have CRUD interface.

```csharp
public interface IFooService : IBaseService<Foo> { }

[Service]
public class FooService : BaseService<Foo, IFooRepository>, IFooService
{
    // Required.
    public FooService(IFooRepository repository) : base(repository) { }
    
    public string? GetPhoneByName(string name) => Repository.GetPhoneByName(name);
}
```

## Formatter

Fabric.Web provides some formatter,
you can use them like this:

### Date formatter

You can add date formatter like this:

```csharp
builder.Services.AddFabricWebService(config, addDateTimeFormatter: true);
```

Before: ``2023-01-01T00:00:00``

After: ``2023-01-01 00:00:00``

### No content formatter

You can add no content formatter like this:

```csharp
builder.Services.AddFabricWebService(config, addNoContentFormatter: true);
```

Before: Return Http 204: No Content.
After: ``null``
