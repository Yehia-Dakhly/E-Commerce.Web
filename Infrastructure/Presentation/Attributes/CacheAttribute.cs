using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    internal class CacheAttribute(int DurationInSeconds = 90) : ActionFilterAttribute //Attribute, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Create Cache Key
            var CacheKey = CreateCacheKey(context.HttpContext.Request);

            // Check Value With Cache Key
            ICacheService CacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var CacheValue = await CacheService.GetAsync(CacheKey);

            // If Exists Return Value
            if (CacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = CacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK,
                };
                return;
            }
            // Return Value is Null
            // Invoke Next 
            var ExeContext = await next.Invoke();
            // Set Value With Cache Key
            if (ExeContext.Result is OkObjectResult result)
            {
                await CacheService.SetAsync(CacheKey, result.Value, TimeSpan.FromSeconds(DurationInSeconds));
            }
        }
        private string CreateCacheKey(HttpRequest Request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(Request.Path + '?');
            foreach (var item in Request.Query.OrderBy(I => I.Key))
            {
                Key.Append($"{item.Key}={item.Value}&");
            }

            return Key.ToString();
        }
    }
}
