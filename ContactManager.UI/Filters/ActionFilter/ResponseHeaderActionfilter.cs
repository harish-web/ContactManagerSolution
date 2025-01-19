using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ActionFilter
{
    #region Syncronize
    /* public class ResponseHeaderActionfilter : IActionFilter,IOrderedFilter
     {
         private readonly ILogger<ResponseHeaderActionfilter> logger;
         private readonly string key;
         private readonly string value;
         public int Order { get; set; }

         public ResponseHeaderActionfilter(ILogger<ResponseHeaderActionfilter> logger,string key, string value,int order)
         {
             this.logger = logger;
             this.key = key;
             this.value = value;
             Order = order;
         }



         public void OnActionExecuted(ActionExecutedContext context)
         {
             logger.LogInformation("{FilterName}.{MethodName}",nameof(ResponseHeaderActionfilter),nameof(OnActionExecuted));
             context.HttpContext.Response.Headers[key] = value;
         }

         public void OnActionExecuting(ActionExecutingContext context)
         {

             logger.LogInformation("On Actin Executing");
         }
     }*/
    #endregion

    public class ResponseHeaderActionfilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<ResponseHeaderActionfilter> logger;
        private readonly string key;
        private readonly string value;
        public int Order { get; set; }

        public ResponseHeaderActionfilter(ILogger<ResponseHeaderActionfilter> logger, string key, string value, int order)
        {
            this.logger = logger;
            this.key = key;
            this.value = value;
            Order = order;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogInformation("On Actin Executing");
            await next();
            logger.LogInformation("{FilterName}.{MethodName}", nameof(ResponseHeaderActionfilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[key] = value;
        }
    }
}
