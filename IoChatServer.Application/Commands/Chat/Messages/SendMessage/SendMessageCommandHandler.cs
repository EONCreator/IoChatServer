using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageResponse>
{
    private IUserService _userService;
    private IHubContext<ChatHub> _chatHub;
    private IRepository _repository;
    private IChatService _chatService;
    
    public SendMessageCommandHandler(
        IUserService userService, 
        IHubContext<ChatHub> chatHub,
        IRepository repository,
        IChatService chatService)
    {
        _userService = userService;
        _chatHub = chatHub;
        _repository = repository;
        _chatService = chatService;
    }

    private async Task SaveMessage(ChatRoom chatRoom, Message message)
    {
        var currentUser = await _userService.GetCurrentUser();
        
        chatRoom.SetLastMessage(message.Text);

        message.SetChatRoomId(chatRoom.Id);
        message.SetSenderName($"{currentUser.FirstName} {currentUser.LastName}");
        message.SetSenderAvatar(currentUser.Avatar);
        message.SetDate(DateTime.UtcNow);

        var messageToSave = new Message(message.Text, message.SenderId, message.ChatRoomId);
        _repository.Entity<Message>().Add(messageToSave);
        
        await _repository.SaveChanges();
    }

    private async Task SendMessageToChatRoom(ChatRoom chatRoom, Message message)
    {
        var userToIds = new List<string>();

        var userId = await _userService.GetCurrentUserId();
        userToIds.Add(userId);
        
        foreach (var user in chatRoom.Users)
            userToIds.Add(user.Id.ToString());
        
        List<string> connectionIds = new List<string>();
        connectionIds.AddRange(ChatHub.GetUsersConnections(userToIds));

        await _chatHub.Clients.Clients(connectionIds).SendAsync("send", message);
    }
    
    public async Task<SendMessageResponse> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatService.GetChatRoom(command.Message.ChatRoomId);
        
        var userInChatRoom = await _chatService.UserInChatRoom(command.Message.ChatRoomId);
        if (!userInChatRoom)
            return null;
        
        await SaveMessage(chatRoom, command.Message);
        await SendMessageToChatRoom(chatRoom, command.Message);
        
        return new SendMessageResponse();
    }
}