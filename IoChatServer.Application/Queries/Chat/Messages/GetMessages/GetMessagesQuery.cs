using MediatR;

namespace IoChatServer.Application.Queries.Chat.Messages.GetMessages;

public class GetMessagesQuery : IRequest<GetMessagesOutput>
{
    public int ChatRoomId { get; }

    public GetMessagesQuery(int chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}