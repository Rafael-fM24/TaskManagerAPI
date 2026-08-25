using Application.Interfaces.Services;
using Application.Mapping;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(
            cfg => { },
            typeof(DomainToDTOMapping).Assembly
        );

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskItemService, TaskItemService>();
        services.AddScoped<ITaskNoteService, TaskNoteService>();

        return services;
    }
}