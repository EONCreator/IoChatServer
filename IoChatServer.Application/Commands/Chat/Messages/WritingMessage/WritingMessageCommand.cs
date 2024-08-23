using MediatR;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class WritingMessageCommand : IRequest<WritingMessageOutput>
{
    public int ChatRoomId { get; set; }

    public WritingMessageCommand(int chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}