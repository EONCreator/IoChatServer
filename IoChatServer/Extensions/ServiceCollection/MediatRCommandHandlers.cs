namespace IoChatServer.Extensions;

public static class MediatRCommandHandlers
{
    public static IServiceCollection AddMediatRCommandHandlers(this IServiceCollection services)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        }

        return services;
    }
}