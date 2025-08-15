using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Domain.Extensions;
public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDomainServiceCollections(this IServiceCollection services)
    {
        return services;
    }

}
