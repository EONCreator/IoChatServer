using MediatR;

namespace IoChatServer.Application.Commands.Chat.FindUser;

public class FindUserCommand : IRequest<FindUserOutput>
{
    public string UserName { get; set; }

    public FindUserCommand(string userName)
    {
        UserName = userName;
    }
}