using System;
namespace DemoMiddleware.Middleware
{
    public class AuthorizedPostMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizedPostMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated && context.Request.Method == "POST")
            {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("No estas permitido hacer acciones POST");
            }
            return _next.Invoke(context);
        }
    }
}

