using System;
using DemoMiddleware.Middleware;

namespace DemoMiddleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizedPost(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthorizedPostMiddleware>();
        }
        public static IApplicationBuilder UseCustomHeaders(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomHeaderMiddleware>();
        }
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
        public static IApplicationBuilder UseTimeLoggingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TimeLoggingMiddleware>();
        }
    }
}

