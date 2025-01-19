using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ResultFilter
{
    public class PersonsListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonsListResultFilter> logger;

        public PersonsListResultFilter(ILogger<PersonsListResultFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            logger.LogInformation("{FiterName}.{MethodNam}", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));
           
            //context.HttpContext.Response.Headers["Last-ModifiedTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            await next();
          // context.HttpContext.Response.Headers["Last-ModifiedTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            logger.LogInformation("{FiterName}.{MethodNam}", nameof(PersonsListResultFilter), nameof(OnResultExecutionAsync));
            
          
        
        }
    }
}
