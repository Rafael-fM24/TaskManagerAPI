using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<ITaskItemService, TaskItemService>();
        
        services.AddScoped<ITaskNoteService, TaskNoteService>();

        return services;
    }
}