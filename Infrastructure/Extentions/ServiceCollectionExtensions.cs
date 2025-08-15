#region Using Directives
using System.Text;
using Application.Services;
using Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Implementations.Persistence.Contexts;
using Domain.Interfaces.UnitOfWorks;
using Infrastructure.Implementations.Persistence.UnitOfWorks;
using Domain.Interfaces.Repositories;
using Infrastructure.Implementations.Persistence.Repositories;
using Application.Configurations.Security;
using Infrastructure.Implementations.Persistence.Repositories.Security;
using Domain.Interfaces.Repositories.Security;
using Microsoft.Extensions.Logging;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Domain.Interfaces.Repositories.Ownership;
using Infrastructure.Implementations.Persistence.Repositories.Ownership;
using Domain.Interfaces.Repositories.Expenditure;
using Infrastructure.Implementations.Persistence.Repositories.Expenditure;
using Infrastructure.Configurations;
using Hangfire;
using Infrastructure.BackgroundJobs.Email;
using Microsoft.Extensions.Hosting;
using Domain.Configurations;
#endregion

namespace Infrastructure.Extentions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServiceCollections(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .RegisterCors()
            .RegisterRepositories()
            .RegisterUnitOfWork()
            .RegisterDateTimeServices()
            .RegisterIdentity()
            .RegisterJwtServices()
            .RegisterBackgroundServices()
            .RegisterEmailServices(configuration)
            .RegisterDBContext(configuration);

        return services;
    }
    private static IServiceCollection RegisterDBContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        string SpendWiseConnectionString = configuration.GetConnectionString("SpendWiseConnection") ?? throw new ApplicationException("Connection string is not initialized!");

        services.AddDbContext<AppDBContext>(options =>
            options
                .UseSqlServer(SpendWiseConnectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information),
            ServiceLifetime.Scoped);


        Console.WriteLine("SpendWiseConnection Registerd:{0}", SpendWiseConnectionString);

        services.AddScoped<DBContext>(provider => provider.GetService<AppDBContext>()!);

        return services;
    }
    private static IServiceCollection RegisterIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

        return services;
    }
    private static IServiceCollection RegisterJwtServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

    private static IServiceCollection RegisterCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("allowall", policy =>
            {
                policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        return services;
    }
    public static IServiceCollection RegisterDateTimeServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();

        return services;
    }
    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        //Base Repository
        services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        //Security Repository
        services.AddScoped(typeof(IRolesRepository), typeof(RolesRepository));

        //Ownership Repository
        services.AddScoped(typeof(IOrganizationRepository), typeof(OrganizationRepository));

        //Expenditure Repository
        services.AddScoped(typeof(IBillsDetailsRepository), typeof(BillDetailsRepository));
        services.AddScoped(typeof(IBillsRepository), typeof(BillsRepository));


        return services;
    }
    private static IServiceCollection RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(IAppUnitOfWork), typeof(AppUnitOfWork));
       
        return services;
    }
    private static IServiceCollection RegisterEmailServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        EmailSettings emailConfig = configuration
        .GetSection("EmailSettings")
        .Get<EmailSettings>() ?? throw new ApplicationException("EmailConfiguration not configured");

        services.AddSingleton(emailConfig);

        services.AddSingleton<IEmailService, EmailService>();

        return services;
    }
    private static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
    {
        services.AddSingleton<IHostedService, EmailBackgroundService>();

        return services;
    }
}