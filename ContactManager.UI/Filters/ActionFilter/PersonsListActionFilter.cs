using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ActionFilter
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
          

            logger.LogInformation("PersonListActionFilter OnExecuted Method");
            IDictionary<string, object?> keyValuePairs = (IDictionary<string, object?>)context.HttpContext.Items["arguments"];

            if (keyValuePairs != null)
            {
                if (keyValuePairs.ContainsKey("searchBy"))
                {
                    var searchBy = keyValuePairs["searchBy"];
                    logger.LogInformation("Search by  {searchBy}",searchBy);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
                if (string.IsNullOrEmpty(searchBy))
                {
                    logger.LogInformation($"Search by {searchBy}"); 
                }
            }
            logger.LogInformation("PersonListActionFilter OnExecuting Method");
        }
    }
}
