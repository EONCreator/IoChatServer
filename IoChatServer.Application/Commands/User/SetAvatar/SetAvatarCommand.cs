using MediatR;
using IoChatServer.Application.Commands.User.CreateUserCommand.User;

namespace IoChatServer.Application.Commands.User.CreateUserCommand;

public class SetAvatarCommand : IRequest<SetAvatarResponse>
{
    public string Base64EncodedImage { get; set; }
}