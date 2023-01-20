using Fabric.Web.Converter;
using Fabric.Web.Formatter;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Web.Branch;

[Branch]
public static class FormatterBranch
{
    [BranchDictionary]
    private static readonly Dictionary<FormatterType, Action<IServiceCollection>> dictionary;

    static FormatterBranch()
    {
        dictionary = new Dictionary<FormatterType, Action<IServiceCollection>>
        {
            { FormatterType.DateTime, service => service.AddDateTimeFormatter() },
            { FormatterType.NoContext, service => service.AddNoContentFormatter() }
        };
    }

    private static void AddDateTimeFormatter(this IServiceCollection service)
    {
        service.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateTimeOutputConverter()));
    }

    private static void AddNoContentFormatter(this IServiceCollection service)
    {
        service.AddMvc(options => options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>());
        service.AddMvc(options => options.OutputFormatters.Add(new HttpNoContentOutputFormatter
        {
            TreatNullValueAsNoContent = false
        }));
    }

    public static void AddFormatter(this IServiceCollection service, FormatterType formatterType) { dictionary[formatterType].Invoke(service); }
}
