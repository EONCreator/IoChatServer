using IoChatServer.Application.Commands.User.CreateUserCommand.User;
using IoChatServer.Services.User;
using MediatR;

namespace IoChatServer.Application.Commands.User.CreateUserCommand;

public class SetAvatarCommand : IRequest<SetAvatarResponse>
{
    public string Base64EncodedImage { get; set; }
}