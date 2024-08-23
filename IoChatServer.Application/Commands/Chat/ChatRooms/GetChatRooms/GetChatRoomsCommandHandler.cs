using MediatR;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class GetChatRoomsCommandHandler : IRequestHandler<GetChatRoomsCommand, GetChatRoomsOutput>
{
    private IChatService _chatService;
    
    public GetChatRoomsCommandHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<GetChatRoomsOutput> Handle(GetChatRoomsCommand command, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatService.GetChatRooms();
        return GetChatRoomsOutput.Success(chatRooms);
    }
}