using MediatR;

namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;

public class CreateChatRoomCommand : IRequest<CreateChatRoomOutput>
{
    public List<string> UserIds { get; set; }

    public CreateChatRoomCommand(List<string> userIds)
    {
        UserIds = userIds;
    }
}