using MediatR;

namespace IoChatServer.Application.Queries.Chat.FindUser;

public class FindUserQuery : IRequest<FindUserOutput>
{
    public string UserName { get; set; }

    public FindUserQuery(string userName)
    {
        UserName = userName;
    }
}