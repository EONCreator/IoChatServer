using IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;
using IoChatServer.Application.Commands.Messages.SendMessageCommand;
using IoChatServer.Application.Commands.User.CreateUserCommand;
using IoChatServer.Application.Commands.User.GetCurrentUser;

namespace IoChatServer.Extensions;

public static class MediatRCommandHandlers
{
    public static IServiceCollection AddMediatRCommandHandlers(this IServiceCollection services)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
        }
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateUserCommandHandler).Assembly, typeof(AuthenticateCommand).Assembly));

        return services;
    }
}