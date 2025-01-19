using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> logger;
        private readonly IHostEnvironment hostEnvironment;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger,IHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogError("ExceptionFilter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}",
                nameof(HandleExceptionFilter),nameof(OnException),context.Exception.GetType().ToString(),context.Exception.Message);
            
            if(hostEnvironment.IsDevelopment())
            context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500 };
        }
    }
}
