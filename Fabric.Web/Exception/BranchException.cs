namespace Fabric.Web.Exception;

public class BranchException : System.Exception
{
    public BranchException() : base("The count of dictionary is not match the enumeration.") { }
}
