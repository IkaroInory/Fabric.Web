namespace Fabric.Web.Exception;

public class BranchDictionaryException : System.Exception
{
    public BranchDictionaryException() : base("The count of branch dictionary is not equals the count of the enum.") { }
}
