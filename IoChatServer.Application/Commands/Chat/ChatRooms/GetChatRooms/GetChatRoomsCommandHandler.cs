using MediatR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

using Domain.Entities;

public class GetChatRoomsCommandHandler : IRequestHandler<GetChatRoomsCommand, GetChatRoomsResponse>
{
    private IRepository _repository;
    private IUserService _userService;
    
    public GetChatRoomsCommandHandler(IRepository repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }
    
    public async Task<GetChatRoomsResponse> Handle(GetChatRoomsCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();
        
        var response = new List<ChatRoomUserModel>();

        var currentUser = await _repository.Entity<User>()
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
                    
                    LastMessage = _repository.Entity<Message>()
                        .Where(m => m.ChatRoomId == chatRoom.Id).Count() != 0 
                        
                        ? _repository.Entity<Message>()
                            .OrderByDescending(m => m.Date)
                        .FirstOrDefault(m => m.ChatRoomId == chatRoom.Id).Text : "",
                    
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