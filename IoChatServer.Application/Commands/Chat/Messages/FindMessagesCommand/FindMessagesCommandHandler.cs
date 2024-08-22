using MediatR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;

public class FindMessagesCommandHandler : IRequestHandler<FindMessagesCommand, FindMessagesResponse>
{
    private IUserService _userService;
    private IRepository _repository;
    private IChatService _chatService;
    
    public FindMessagesCommandHandler(
        IUserService userService,
        IRepository repository,
        IChatService chatService)
    {
        _userService = userService;
        _repository = repository;
        _chatService = chatService;
    }
    
    public async Task<FindMessagesResponse> Handle(FindMessagesCommand command, CancellationToken cancellationToken)
    {
        var messages = await _chatService.GetMessagesOfChatRoom(command.ChatRoomId, m => m.Text.ToLower().Contains(command.Text.ToLower()));
        
        var messageList = new List<MessageModel>();

        foreach (var message in messages)
            messageList.Add(new MessageModel(message.Id, message.Text, message.Date, message.SenderId, $"{message.Sender.FirstName} {message.Sender.LastName}", message.Sender.Avatar));

        return new FindMessagesResponse(messageList);
    }
}