using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ResourceFilter
{
    public class TokenResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<TokenResultFilter> logger;

        public TokenResultFilter(ILogger<TokenResultFilter> logger)
        {
            this.logger = logger;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
            await next();
        }
    }
}
