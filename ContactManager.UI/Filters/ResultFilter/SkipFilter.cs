using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ResultFilter
{
    public class SkipFilter :Attribute ,IFilterMetadata
    {
    }
}
