using TradeSuite.Api.Endpoints;
using TradeSuite.Api.Middleware;
using TradeSuite.Application;
using TradeSuite.Infrastructure;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapClientEndpoints();
app.MapSupplierEndpoints();
app.MapSupplierPartEndpoints();
app.UseHttpsRedirection();

app.Run();
