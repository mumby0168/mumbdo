using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.DependencyInjection;
using Mumbdo.Application.Jwt;
using Mumbdo.Application.Services;
using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordService, Pbkdf2PasswordService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IItemGroupService, ItemGroupService>();
            services.AddSingleton<ITaskService, TaskService>();
            return services;
        }
    }
}  