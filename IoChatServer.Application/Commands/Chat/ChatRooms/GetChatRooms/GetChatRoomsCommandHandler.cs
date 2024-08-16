using MediatR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Data;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class GetChatRoomsCommandHandler : IRequestHandler<GetChatRoomsCommand, GetChatRoomsResponse>
{
    private IRepository _repository;
    private IUserService _userService;
    private AppDbContext _dbContext;
    
    public GetChatRoomsCommandHandler(IRepository repository, IUserService userService, AppDbContext dbContext)
    {
        _repository = repository;
        _userService = userService;
        _dbContext = dbContext;
    }
    
    public async Task<GetChatRoomsResponse> Handle(GetChatRoomsCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();
        
        var response = new List<ChatRoomUserModel>();

        var currentUser = await _dbContext.Users
            .Include(u => u.ChatRooms)
            .ThenInclude(c => c.Users)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        foreach (var chatRoom in currentUser.ChatRooms)
        {
            var chatIsGroup = chatRoom.Users.Count > 2; // If chat room's users count > 2, it's group
            foreach (var user in chatRoom.Users.Where(u => u.Id.ToString() != userId))
            {
                var chatRoomName = chatIsGroup ? "Group" : $"{user.FirstName} {user.LastName}";
                
                response.Add(new ChatRoomUserModel()
                {
                    Id = user.Id.ToString(),
                    Avatar = user.Avatar,
                    ChatRoomId = chatRoom.Id,
                    ChatRoomName = chatRoomName,
                    LastMessage = "Последнее сообщение",
                    UnreadMessages = 0,
                    Online = ChatHub.Connections.GetConnections(user.Id.ToString()).Count() != 0
                });

                if (chatIsGroup)
                    break;
            }
        }

        return new GetChatRoomsResponse(response);
    }
}