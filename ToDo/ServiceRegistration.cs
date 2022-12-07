using ToDo.Infrastructure;
using ToDo.Services;
using ToDo.ViewModels;
using ToDo.Views;

namespace ToDo;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ToDoDb>();

        // services.AddSingleton<IToDoService, ToDoInMemoryService>();
        services.AddSingleton<IToDoService, ToDoSqLiteService>();

        services.AddSingleton<ToDoViewModel>();
        services.AddSingleton<ToDoPage>();

        return services;
    }
}
