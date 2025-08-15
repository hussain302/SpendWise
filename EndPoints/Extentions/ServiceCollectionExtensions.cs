#region Using Directives
using Shared.Security;
using Domain.Interfaces;
using EndPoints.Infrastructure.ApiConfigs;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Application.Services;
using Domain.Configurations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
#endregion
namespace EndPoints.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndPointServiceCollections(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterCurrentUser()
                .RegisterMemoryCache()
                .RegisterAPIVersioning()
                //.RegisterSwagger()
                .RegisterScalar()
                .RegisterAuthentication(configuration)
                .RegisterKeys(configuration)
                .RegisterSerilog(configuration);

        return services;
    }
    public static IServiceCollection RegisterMemoryCache(this IServiceCollection services)
    {
        services.AddMemoryCache();

        return services;
    }  
    private static IServiceCollection RegisterCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor()
                .AddSingleton<ICurrentUser, CurrentUser>();

        return services;
    }
    public static IServiceCollection RegisterAPIVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        })
            .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return services;
    }
    private static IServiceCollection RegisterKeys(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PasswordSecurityKeyModel>(configuration.GetSection("PasswordSecurityKey"));
        return services;
    }
    private static IServiceCollection RegisterScalar(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        // Register Scalar services for API documentation
        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
    private static IServiceCollection RegisterSerilog(this IServiceCollection services, 
        IConfiguration configuration)
    {
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration) // ✅ Reads settings from appsettings.json
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logging/SerilogFiles/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });

        return services;
    }
    private static IServiceCollection RegisterAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"] ?? "AlternativeKey"))
            };
        });

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
    //Use this in case of swagger
    //private static IServiceCollection RegisterSwagger(this IServiceCollection services)
    //{

    //    services.AddSwaggerGen(c =>
    //    {

    //        c.SwaggerDoc("v1", new OpenApiInfo
    //        {
    //            Title = "Project Swagger",
    //            Version = "v1",
    //            Description = "SpendWise is an Expense Calculator",
    //            Contact = new OpenApiContact
    //            {
    //                Name = "Muhammad Hussain",
    //                Email = "hussainsaqib302@gmail.com",
    //                Url = new Uri("https://www.linkedin.com/in/muhammadhussain01/")
    //            }
    //        });

    //        c.SwaggerDoc("v2", new OpenApiInfo
    //        {
    //            Title = "Project Swagger",
    //            Version = "v2",
    //            Description = "SpendWise v2 APIs",
    //            Contact = new OpenApiContact
    //            {
    //                Name = "Muhammad Hussain",
    //                Email = "hussainsaqib302@gmail.com",
    //                Url = new Uri("https://www.linkedin.com/in/muhammadhussain01/")
    //            }
    //        });

    //        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //        {
    //            In = ParameterLocation.Header,
    //            Description = "Please insert JWT token into field",
    //            Name = "Authorization",
    //            Type = SecuritySchemeType.Http,
    //            BearerFormat = "JWT",
    //            Scheme = "bearer"
    //        });

    //        c.OperationFilter<SecurityRequirementsOperationFilter>();
    //        c.DocInclusionPredicate((version, apiDescription) =>
    //        {
    //            if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

    //            var versions = methodInfo.DeclaringType
    //                .GetCustomAttributes(true)
    //                .OfType<ApiVersionAttribute>()
    //                .SelectMany(attr => attr.Versions);

    //            return versions.Any(v => $"v{v}" == version);
    //        });
    //    });

    //    return services;
    //}
}