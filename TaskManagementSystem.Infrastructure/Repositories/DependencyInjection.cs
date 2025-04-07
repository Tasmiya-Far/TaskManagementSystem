using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Intefaces;
using TaskManagementSystem.Infrastructure.Services;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //adding all interface scope to services

            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //services
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
