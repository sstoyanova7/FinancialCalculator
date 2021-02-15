using FinancialCalculator.BL.Exceptions;
using FinancialCalculator.Host.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialCalculator.Host.Config
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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
            HttpStatusCode status;
            string message;
            var stackTrace = String.Empty;
            Error error = new Error();

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(NotFoundException))
            {
                error.time = DateTime.Now;
                error.errorMessage = exception.Message;
                error.statusCode = HttpStatusCode.NotFound.ToString();
                status = HttpStatusCode.NotFound;

            }
            else if (exceptionType == typeof(BadRequestException))
            {
                error.time = DateTime.Now;
                message = exception.Message;
                error.errorMessage = exception.Message;
                status = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(AlreadyExistException))
            {
                error.time = DateTime.Now;
                message = exception.Message;
                error.errorMessage = exception.Message;
                status = HttpStatusCode.Conflict;
            }
            else
            {
                error.time = DateTime.Now;
                status = HttpStatusCode.InternalServerError;
                error.statusCode = HttpStatusCode.InternalServerError.ToString();
                error.errorMessage = exception.Message;
                message = exception.Message;
            }

            var result = JsonSerializer.Serialize(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }

    public class Error
    {
        public DateTime time { get; set; }
        public string errorMessage { get; set; }
        public string statusCode{ get; set; }
        
    }
}
