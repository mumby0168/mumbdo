using System;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Infrastructure.Documents;
using Mumbdo.Infrastructure.Repositories;

namespace Mumbdo.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddConvey()
                .AddMongo()
                .AddMongoRepository<UserDocument, Guid>("users");
            services.AddTransient<IUserRepository, UserRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            
            return app;
        }
        
    }
}
