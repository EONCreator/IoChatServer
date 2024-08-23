using IoChatServer.Abstractions;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Chat;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageOutput>
{
    private readonly IUserService _userService;
    private readonly IHubContext<ChatHub> _chatHub;
    private readonly IRepository _repository;
    private readonly IChatService _chatService;
    
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
        chatRoom.SetLastMessageDateTime(DateTime.UtcNow);

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
        var userToIds = new List<string>() { await _userService.GetCurrentUserId() };
        
        foreach (var user in chatRoom.Users)
            userToIds.Add(user.Id.ToString());

        await _chatHub.Clients.Clients(
                ChatHub.GetUsersConnections(userToIds))
            .SendAsync(ChatEvents.SEND_MESSAGE, message);
    }
    
    public async Task<SendMessageOutput> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatService.GetChatRoom(command.Message.ChatRoomId);
        if (chatRoom == null)
            return SendMessageOutput.Failure(ChatErrors.DoesNotExists);
        
        var userInChatRoom = await _chatService.UserInChatRoom(command.Message.ChatRoomId);
        if (!userInChatRoom)
            return SendMessageOutput.Failure(ChatErrors.UserNotInChatRoom);
        
        await SaveMessage(chatRoom, command.Message);
        await SendMessageToChatRoom(chatRoom, command.Message);
        
        return SendMessageOutput.Success();
    }
}