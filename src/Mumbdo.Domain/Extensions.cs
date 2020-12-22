using Microsoft.Extensions.DependencyInjection;
using Mumbdo.Domain.Aggregates;

namespace Mumbdo.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddSingleton<IUserAggregate, UserAggregate>();
            services.AddSingleton<IItemGroupAggregate, ItemGroupAggregate>();
            return services;
        }
        
        
    }
}