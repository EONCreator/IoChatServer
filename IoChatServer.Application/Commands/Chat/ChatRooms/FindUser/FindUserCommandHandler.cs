using IoChatServer.Application.Commands.Chat.Messages.FindMessagesCommand;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Chat;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Chat.FindUser;

public class FindUserCommandHandler : IRequestHandler<FindUserCommand, FindUserOutput>
{
    private IRepository _repository;
    private IUserService _userService;
    private IChatService _chatService;
    
    public FindUserCommandHandler(
        IRepository repository, 
        IUserService userService,
        IChatService chatService)
    {
        _repository = repository;
        _userService = userService;
        _chatService = chatService;
    }
    
    public async Task<FindUserOutput> Handle(FindUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _repository.Entity<Domain.Entities.User>()
            .FirstOrDefaultAsync(u => u.UserName == command.UserName);
        
        if (user == null)
            return FindUserOutput.Failure(UserErrors.NotFound);
        
        var id = user.Id.ToString();

        var chatRoom = await _chatService.GetChatRoom(new List<string> { id });
        if (chatRoom == null)
            chatRoom = new ChatRoom();

        var userName = user.UserName;
        var avatar = user.Avatar;
        var fullName = $"{user.FirstName} {user.LastName}";
        var online = ChatHub.Connections.GetConnections(id).Count() > 0;
        
        var result = new FindUserClientModel(
            id,
            chatRoom != null ? chatRoom.Id : null, 
            avatar,
            userName, 
            fullName,
            online);
        
        return FindUserOutput.Success(result);
    }
}