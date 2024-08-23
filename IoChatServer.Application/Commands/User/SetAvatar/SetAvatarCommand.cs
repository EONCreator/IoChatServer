using MediatR;
using IoChatServer.Application.Commands.User.CreateUserCommand.User;
using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Commands.User.CreateUserCommand;

public class SetAvatarCommand : IRequest<SucceededResult>
{
    public string Base64EncodedImage { get; set; }
}