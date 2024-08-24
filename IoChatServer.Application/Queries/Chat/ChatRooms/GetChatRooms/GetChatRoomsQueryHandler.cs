using MediatR;
using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class GetChatRoomsQueryHandler : IRequestHandler<GetChatRoomsQuery, GetChatRoomsOutput>
{
    private IChatService _chatService;
    
    public GetChatRoomsQueryHandler(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<GetChatRoomsOutput> Handle(GetChatRoomsQuery query, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatService.GetChatRooms();
        return GetChatRoomsOutput.Success(chatRooms);
    }
}