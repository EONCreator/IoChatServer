using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageResponse>
{
    private IUserService _userService;
    private IHubContext<ChatHub> _chatHub;
    private IRepository _repository;
    
    public SendMessageCommandHandler(
        IUserService userService, 
        IHubContext<ChatHub> chatHub,
        IRepository repository)
    {
        _userService = userService;
        _chatHub = chatHub;
        _repository = repository;
    }
    
    public async Task<SendMessageResponse> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        List<string> ids = new List<string>();
        
        // Connections of current user
        var userId = await _userService.GetCurrentUserId();
        var currentUser = await _repository.Entity<Domain.Entities.User>()
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        
        var chatRoom = await _repository.Entity<ChatRoom>()
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == command.Message.ChatRoomId);

        command.Message.ChatRoomId = chatRoom.Id;
        command.Message.SenderName = $"{currentUser.FirstName} {currentUser.LastName}";
        command.Message.SenderAvatar = currentUser.Avatar;

        command.Message.Date = DateTime.UtcNow;

        var messageToSave = new Message(
            command.Message.Text, 
            command.Message.SenderId, 
            command.Message.ChatRoomId);
        _repository.Entity<Message>().Add(messageToSave);
        
        await _repository.SaveChanges();
        
        // Connections of getting users in chat room
        var userToIds = new List<string>();
        userToIds.Add(userId);
        
        foreach (var user in chatRoom.Users)
            userToIds.Add(user.Id.ToString());
        
        ids.AddRange(ChatHub.GetUsersConnections(userToIds));

        await _chatHub.Clients.Clients(ids).SendAsync("send", command.Message);
        
        return new SendMessageResponse();
    }
}