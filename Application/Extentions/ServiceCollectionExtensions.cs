using MediatR;
using Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Application.Features.Security.Identity.Validations;
using FluentValidation;

namespace Application.Extentions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServiceCollections(this IServiceCollection services)
    {
        services.RegisterMediatR()
                .RegisterFluventValidation();

        return services;
    }
  
    private static IServiceCollection RegisterMediatR(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IAssemblyMaker>();

        services.AddMediatR(cfg =>
        {
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.RegisterServicesFromAssembly(typeof(IAssemblyMaker).Assembly); 
        });

        return services;
    }
    private static IServiceCollection RegisterFluventValidation(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
