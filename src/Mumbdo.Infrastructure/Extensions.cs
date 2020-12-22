using System;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Infrastructure.Documents;
using Mumbdo.Infrastructure.Repositories;

namespace Mumbdo.Infrastructure
{
    
    public static class Extensions
    {
        private const string DatabaseName = "mumbdo-cosmos-db";
        private const string LocalDevConnectionString = "mongodb://localhost:27017";
        private const string KeyVaultSecretKey = "db-prod";        
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            services
                .AddConvey()
                .AddMongo(builder => builder.WithDatabase(DatabaseName)
                    .WithConnectionString(environment.IsProduction() ? configuration[KeyVaultSecretKey] : LocalDevConnectionString))
                .AddMongoRepository<UserDocument, Guid>("users")
                .AddMongoRepository<RefreshTokenDocument, Guid>("refresh-tokens");
            
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }

    }
}
