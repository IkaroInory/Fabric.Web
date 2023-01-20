using System.Reflection;
using Fabric.Web.Branch;
using Fabric.Web.Exception;

namespace Fabric.Web;

internal class PreLaunchCheck
{
    private static void BranchCheck()
    {
        var branches = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes().Where(type => type.IsDefined(typeof(BranchAttribute))));
        foreach (var branch in branches)
        {
            var dictionaryFields = branch.GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(field => field.IsDefined(typeof(BranchDictionaryAttribute))).ToList();

            if (dictionaryFields.Count != 1)
            {
                throw new BranchDictionaryException();
            }

            var dictionaryField = dictionaryFields.First();

            var keyEnumType = dictionaryField.FieldType.GenericTypeArguments.First();
            dynamic dictionary = dictionaryField.GetValue(null)!;

            if (keyEnumType.GetFields(BindingFlags.Static | BindingFlags.Public).Length != dictionary.Count)
            {
                throw new BranchException();
            }
        }
    }

    internal static void CheckAll()
    {
        var methods = typeof(PreLaunchCheck).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(method => method.Name != "CheckAll");
        foreach (var method in methods)
        {
            method.Invoke(null, null);
        }
    }
}
