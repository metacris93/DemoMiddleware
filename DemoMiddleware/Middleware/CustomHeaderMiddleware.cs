using System;
namespace DemoMiddleware.Middleware
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(state =>
            {
                if (state is HttpContext httpContext)
                {
                    httpContext.Response.Headers.Add("Custom-Middleware-Value", DateTime.Now.ToString());
                }
                return Task.CompletedTask;
            }, context);
            return _next.Invoke(context);
        }
    }
}

