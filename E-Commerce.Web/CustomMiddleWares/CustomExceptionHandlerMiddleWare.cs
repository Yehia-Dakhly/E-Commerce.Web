using DomainLayer.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<CustomExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                await HandleEndPointNotFoundAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong!");
                // Set StatusCode For Response
                //context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                //context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Set Content Type
            // context.Response.ContentType = "application/json";
            // Response Object
            var Response = new ErrorToReturn()
            {
                ErrorMessage = ex.Message
            };
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException badRequestException => GetBadRequestErrors(badRequestException, Response),
                _ => StatusCodes.Status500InternalServerError
            };

            /// Return Object As JSON - Serialize
            //var ResponseToReturn = JsonSerializer.Serialize(Response);
            //await context.Response.WriteAsync(ResponseToReturn);
            Response.StatusCode = context.Response.StatusCode;
            await context.Response.WriteAsJsonAsync(Response);
        }

        private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleEndPointNotFoundAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {context.Request.Path} is Not Found!"
                };
                await context.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
