using Microsoft.Extensions.DependencyInjection;
using Mumbdo.Application.Services;

namespace Mumbdo.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordService, Pbkdf2PasswordService>();
            services.AddSingleton<IUserService, UserService>();
            return services;
        }
        
    }
}  