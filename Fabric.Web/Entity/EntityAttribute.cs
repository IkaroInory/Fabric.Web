namespace Fabric.Web.Entity;

/// <summary>
/// Mark a class as an entity in the database.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EntityAttribute : Attribute { }
