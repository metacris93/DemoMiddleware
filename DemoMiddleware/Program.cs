using DemoMiddleware;
using DemoMiddleware.Configurations;
using DemoMiddleware.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<RequestResponseLoggingMiddleware>();
builder.Services.AddTransient<TimeLoggingMiddleware>();
var app = builder.Build();

var middlewareSettings = builder.Configuration.GetSection("MiddlewareSettings").Get<MiddlewareSettings>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
//If you want to invoke a piece of middleware but not end the pipeline, use app.Use()
//This method chains middleware so that you can modify the response without immediately returning it.
app.Use(async (context, next) =>
{
    context.Response.OnStarting(state =>
    {
        if (state is HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("OnStarting-One", "1");
        }
        return Task.CompletedTask;
    }, context);
    await next(context);
});

app.Use(async (context, next) =>
{
    context.Response.OnStarting(state =>
    {
        if (state is HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("OnStarting-Two", "2");
        }
        return Task.CompletedTask;
    }, context);
    await next(context);
});
//app.UseAuthorizedPost();
app.UseCustomHeaders();
app.UseRequestResponseLogging();
if (middlewareSettings.UseTimeLoggingMiddleware)
    app.UseTimeLoggingMiddleware();
app.Run();

