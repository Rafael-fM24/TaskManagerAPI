using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        
        services.AddScoped<ITaskNoteRepository, TaskNoteRepository>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        
        return services;
    }
}