using System;
using System.Diagnostics;

namespace DemoMiddleware.Middleware
{
    public class TimeLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            await next(context);
            watch.Stop();
            Console.WriteLine($"Time to execute: {watch.ElapsedMilliseconds} milliseconds");
        }
    }
}
