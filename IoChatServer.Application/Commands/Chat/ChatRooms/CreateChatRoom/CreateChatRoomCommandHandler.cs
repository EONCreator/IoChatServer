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

namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;
using Domain.Entities;

public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, CreateChatRoomOutput>
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

    public async Task SaveChatRoom(ChatRoom chatRoom, List<string> userIds)
    {
        chatRoom.Users.AddRange(_repository
            .Entity<User>()
            .Where(u => userIds.Contains(u.Id.ToString())));

        _repository.Entity<ChatRoom>().Add(chatRoom);
        await _repository.SaveChanges();
    }

    public async Task<CreateChatRoomClientModel> SendChatRoom(ChatRoom chatRoom, List<string> userIds)
    {
        var userId = await _userService.GetCurrentUserId();
        var chatId = chatRoom.Id;

        var isGroup = chatRoom.Users.Count > 2;
        var chatRoomUser = chatRoom.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        var chatRoomName = isGroup
            ? chatRoom.Name
            : $"{chatRoomUser.FirstName} {chatRoomUser.LastName}";
        var chatRoomAvatar = isGroup
            ? chatRoom.Avatar
            : chatRoomUser.Avatar;
        
        var response = new CreateChatRoomClientModel(
            chatId,  
            chatRoom.LastMessage, 
            chatRoomName, 
            chatRoomAvatar);
        
        await _chatHub.Clients.Clients(
                ChatHub.GetUsersConnections(userIds.Where(i => i != userId).ToList()))
            .SendAsync(ChatEvents.CREATE_CHAT, response);

        return response;
    }
    
    public async Task<CreateChatRoomOutput> Handle(CreateChatRoomCommand command, CancellationToken cancellationToken)
    {
        var chatRoomIsExists = await _chatService.ChatRoomIsExists(command.UserIds.ToList());
        
        if (chatRoomIsExists)
            return CreateChatRoomOutput.Failure(ChatErrors.AlreadyExists);

        var chatRoom = new ChatRoom();
        
        await SaveChatRoom(chatRoom, command.UserIds);
        var response = await SendChatRoom(chatRoom, command.UserIds);
        
        return CreateChatRoomOutput.Success(response);
    }
}