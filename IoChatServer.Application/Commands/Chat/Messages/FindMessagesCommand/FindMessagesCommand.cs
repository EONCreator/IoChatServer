using IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;
using MediatR;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesCommand : IRequest<FindMessagesOutput>
{
    public int ChatRoomId { get; }
    public string Text { get; }

    public FindMessagesCommand(int chatRoomId, string text)
    {
        ChatRoomId = chatRoomId;
        Text = text;
    }
}