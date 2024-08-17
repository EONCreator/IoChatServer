using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;
using Domain.Entities;

public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, CreateChatRoomResponse>
{
    private IRepository _repository;
    private IHubContext<ChatHub> _chatHub;
    private IUserService _userService;
    private IChatService _chatService;
    
    public CreateChatRoomCommandHandler(
        IRepository repository, 
        IHubContext<ChatHub> chatHub,
        IUserService userService,
        IChatService chatService)
    {
        _repository = repository;
        _chatHub = chatHub;
        _userService = userService;
        _chatService = chatService;
    }
    
    public async Task<CreateChatRoomResponse> Handle(CreateChatRoomCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();

        var chatRoomIsExists = await _chatService.ChatRoomIsExists(command.Ids.ToList());
        
        if (chatRoomIsExists)
            return null;

        var chatRoom = new ChatRoom();

        /*foreach (var id in command.Ids)
        {
            var user = await _repository.Entity<User>()
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);
            
            chatRoom.Users.Add(user);
        }*/
        
        chatRoom.Users.AddRange(_repository
            .Entity<User>()
            .Where(u => command.Ids.Contains(u.Id.ToString())));

        _repository.Entity<ChatRoom>().Add(chatRoom);
        await _repository.SaveChanges();
        
        List<string> ids = new List<string>();
        
        // Connections of getting users in chat room
        var userToIds = new List<IEnumerable<string>>();
        
        foreach (var id in command.Ids.Where(i => i != userId))
            userToIds.Add(ChatHub.Connections.GetConnections(id));
        
        foreach (var user in userToIds)
        {
            foreach (var userConnection in user)
            {
                ids.Add(userConnection);
            }
        }

        var chatId = chatRoom.Id;

        var isGroup = chatRoom.Users.Count > 2;
        var chatRoomUser = chatRoom.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        var chatRoomName = isGroup
            ? "Group" 
            : $"{chatRoomUser.FirstName} {chatRoomUser.LastName}";
        var chatRoomAvatar = isGroup
            ? null
            : chatRoomUser.Avatar;
        
        var response = new CreateChatRoomResponse(chatId,  "Последнее сообщение", chatRoomName, chatRoomAvatar);
        
        await _chatHub.Clients.Clients(ids)
            .SendAsync("create_chat", response);
        
        return response;
    }
}