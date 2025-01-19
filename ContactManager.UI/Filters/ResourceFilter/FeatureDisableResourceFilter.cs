using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDOperation.Filters.ResourceFilter
{
    public class FeatureDisableResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisableResourceFilter> logger;
        private readonly bool isDisabled;

        public FeatureDisableResourceFilter(ILogger<FeatureDisableResourceFilter> logger,bool isDisabled = true)
        {
            this.logger = logger;
            this.isDisabled = isDisabled;
        }
     

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            logger.LogInformation("{FilterName}.{MethodName}",nameof(FeatureDisableResourceFilter),nameof(OnResourceExecutionAsync));
            if (isDisabled)
            {
               // context.Result = new NotFoundResult();//404
                context.Result = new StatusCodeResult(501);
            }
            else
            await next();

            logger.LogInformation("{FilterName}.{MethodName}", nameof(FeatureDisableResourceFilter), nameof(OnResourceExecutionAsync));
        }
    }
}
