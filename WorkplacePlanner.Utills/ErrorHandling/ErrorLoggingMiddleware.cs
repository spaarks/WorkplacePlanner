using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkplacePlanner.Utills.ErrorHandling
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorLoggingMiddleware(RequestDelegate next)
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
                LogError(ex);
                throw ex;
            }
        }

        private static void LogError(Exception ex)
        {
            //loggerFactory.CreateLogger
            //TODO
        }
    }
}
