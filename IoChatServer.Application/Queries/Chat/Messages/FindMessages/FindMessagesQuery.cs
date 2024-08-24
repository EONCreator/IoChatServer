using MediatR;

namespace IoChatServer.Application.Queries.Chat.Messages.FindMessages;

public class FindMessagesQuery : IRequest<FindMessagesOutput>
{
    public int ChatRoomId { get; }
    public string Text { get; }

    public FindMessagesQuery(int chatRoomId, string text)
    {
        ChatRoomId = chatRoomId;
        Text = text;
    }
}