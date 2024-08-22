using MediatR;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class GetChatRoomsCommandHandler : IRequestHandler<GetChatRoomsCommand, GetChatRoomsResponse>
{
    private IChatService _chatService;
    
    public GetChatRoomsCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<GetChatRoomsResponse> Handle(GetChatRoomsCommand command, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatService.GetChatRooms();
        return new GetChatRoomsResponse(chatRooms);
    }
}