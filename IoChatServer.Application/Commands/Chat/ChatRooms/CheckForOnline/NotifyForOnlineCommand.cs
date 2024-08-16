using MediatR;

namespace IoChatServer.Application.Commands.Chat.FindUser;

public class NotifyForOnlineCommand : IRequest<NotifyForOnlineResponse>
{
    public NotifyForOnlineCommand()
    {
        
    }
}