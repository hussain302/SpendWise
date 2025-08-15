#region Usings Directives
using Serilog;
using Domain.Extensions;
using Application.Middlewares;
using EndPoints.Extentions;
using Infrastructure.Extentions;
using EndPoints.Infrastructure.Middlewares;
using Application.Extentions;
using Scalar.AspNetCore;
using Infrastructure.Implementations.Persistence.Contexts.Seeding;
using Shared.Exceptions;
#endregion

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

string encrptionKey = configuration.GetSection("PasswordSecurityKey:Key")?.Value?.ToString() 
    ?? throw new InternalServerErrorException("PasswordSecurityKey is not configured properly!");

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDomainServiceCollections();

builder.Services.AddApplicationServiceCollections();

builder.Services.AddInfrastructureServiceCollections(configuration);

builder.Services.AddEndPointServiceCollections(configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Configure Swagger
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });

    app.MapScalarApiReference("openapi/scalar/docs/", opt =>
    {
        opt.Title = "SpendWise OpenAPIs";
        opt.Theme = ScalarTheme.Default;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); 

app.UseAuthorization();

app.UseCors("allowall");

app.UseMiddleware<RateLimitMiddleware>();

app.UseSerilogRequestLogging();

app.Use(async (context, next) =>
{
    Log.Information("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
    Log.Information("Finished handling request.");
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services, encrptionKey);
}

app.Run();

