using MediatR;

namespace IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

public class GetMessagesCommand : IRequest<GetMessagesOutput>
{
    public int ChatRoomId { get; }

    public GetMessagesCommand(int chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}