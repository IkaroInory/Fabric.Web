namespace Fabric.Web.Exception;

/// <summary>
/// The count of branch dictionary is not equals the count of the enum.
/// </summary>
public class BranchDictionaryException : System.Exception
{
    public BranchDictionaryException() : base("The count of branch dictionary is not equals the count of the enum.") { }
}
