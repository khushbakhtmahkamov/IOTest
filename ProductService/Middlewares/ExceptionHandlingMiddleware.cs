using Microsoft.AspNetCore.Http;
using ProductService.Utils;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(new { });

            if (exception is BusinessException businessEx)
            {
                switch (businessEx.BusinessExceptionCode)
                {
                    case BusinessExceptionCode.NOT_FOUND:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        result = JsonSerializer.Serialize(new { code = businessEx.BusinessExceptionCode.ToString(), message = businessEx.Message });
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = JsonSerializer.Serialize(new { code = BusinessExceptionCode.INTERNAL_ERROR.ToString(), message = "An unexpected error occurred." });
                        break;

                }

            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(new { code = BusinessExceptionCode.INTERNAL_ERROR.ToString(), message = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
