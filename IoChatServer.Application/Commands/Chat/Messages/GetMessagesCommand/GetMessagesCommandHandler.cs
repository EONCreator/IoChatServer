using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Chat;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.Messages.GetMessagesCommand;

public class GetMessagesCommandHandler : IRequestHandler<GetMessagesCommand, GetMessagesOutput>
{
    private UserManager<Domain.Entities.User> _userManager;
    private IUserService _userService;
    private IRepository _repository;
    private IChatService _chatService;
    
    public GetMessagesCommandHandler(
        UserManager<Domain.Entities.User> userManager, 
        IUserService userService,
        IRepository repository,
        IChatService chatService)
    {
        _userManager = userManager;
        _userService = userService;
        _repository = repository;
        _chatService = chatService;
    }
    
    public async Task<GetMessagesOutput> Handle(GetMessagesCommand command, CancellationToken cancellationToken)
    {
        var userInChatRoom = await _chatService.UserInChatRoom(command.ChatRoomId);
        if (!userInChatRoom)
            return GetMessagesOutput.Failure(ChatErrors.UserNotInChatRoom);
        
        var messages = await _chatService.GetMessagesOfChatRoom(command.ChatRoomId);
        
        var messageList = new List<MessageClientModel>();

        foreach (var message in messages)
            messageList.Add(new MessageClientModel(
                message.Id, 
                message.Text, 
                message.Date, 
                message.SenderId, 
                $"{message.Sender.FirstName} {message.Sender.LastName}", 
                message.Sender.Avatar));
        
        return GetMessagesOutput.Success(messageList);
    }
}