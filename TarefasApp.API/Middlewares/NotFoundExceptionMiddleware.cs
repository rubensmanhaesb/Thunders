using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TarefasApp.Domain.Exceptions;

namespace TarefasApp.API.Middlewares
{
    public class NotFoundExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (TarefaNotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
        }

        private static Task HandleNotFoundExceptionAsync(HttpContext context, TarefaNotFoundException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";


            var errorResponse = new
            {
                Details = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
