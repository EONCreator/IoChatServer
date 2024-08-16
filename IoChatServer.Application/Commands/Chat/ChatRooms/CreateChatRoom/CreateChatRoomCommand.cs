using MediatR;

namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;

public class CreateChatRoomCommand : IRequest<CreateChatRoomResponse>
{
    public ICollection<string> Ids { get; set; }

    public CreateChatRoomCommand(ICollection<string> ids)
    {
        Ids = ids;
    }
}