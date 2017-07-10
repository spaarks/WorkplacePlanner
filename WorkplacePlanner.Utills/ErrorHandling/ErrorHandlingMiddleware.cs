using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using WorkplacePlanner.Utills.CustomExceptions;

namespace WorkplacePlanner.Utills.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            string errorMsg = string.Empty;

            if (exception is WorkplacePlannerException)
            {
                errorMsg = exception.ToString();
            }
            else if (exception is NotImplementedException)
            {
                code = HttpStatusCode.NotImplemented;
                errorMsg = "Feature you are requesting is not implemented yet.";
            }
            else
                errorMsg = "Unknown error occured while processing the request.";

            /*
            if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (exception is MyException) code = HttpStatusCode.BadRequest;
            */

            var result = JsonConvert.SerializeObject(new { error = errorMsg });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}