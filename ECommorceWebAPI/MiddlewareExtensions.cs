using ECommorceWebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace YourProject.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}