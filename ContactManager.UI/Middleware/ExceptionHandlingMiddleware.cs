using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CRUDOperation.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

       
        public ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.logger = logger;
            _next = next;
            
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                if(ex.InnerException != null)
                {
                    //Log for analisys
                    logger.LogError("{Exceptiontype}= {ExceptionMessage} " ,ex.InnerException.GetType().ToString(),ex.InnerException.Message);     
                }
                else
                {
                    //log for analysis
                    logger.LogError("{Exceptiontype}= {ExceptionMessage} ", ex.GetType().ToString(), ex.Message);
                }
                //user Friendly message to user
                /* httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Error occured");*/
                //Important need to show in ui need to rethrow and will be caught by built in exception handler middlewar 
                //again and show it in UI Error View
                throw;   
            }
            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
