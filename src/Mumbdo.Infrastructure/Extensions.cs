using System;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Interfaces.Utilities;
using Mumbdo.Infrastructure.Documents;
using Mumbdo.Infrastructure.Repositories;
using Mumbdo.Infrastructure.Utilities;

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
                    .WithConnectionString(environment.IsProduction()
                        ? configuration[KeyVaultSecretKey]
                        : LocalDevConnectionString))
                .AddMongoRepository<UserDocument, Guid>("users")
                .AddMongoRepository<RefreshTokenDocument, Guid>("refresh-tokens")
                .AddMongoRepository<ItemGroupDocument, Guid>("item-groups")
                .AddMongoRepository<TaskDocument, Guid>("tasks");
            
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IItemGroupRepository, ItemGroupRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IEntityValidator, EntityValidator>();
            return services;
        }

    }
}
