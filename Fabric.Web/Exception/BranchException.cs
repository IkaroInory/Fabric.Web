namespace Fabric.Web.Exception;

/// <summary>
/// The count of dictionary is not match the enumeration.
/// </summary>
public class BranchException : System.Exception
{
    public BranchException() : base("The count of dictionary is not match the enumeration.") { }
}
