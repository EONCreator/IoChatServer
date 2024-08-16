using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IoChatServer.Application.Commands.Chat.FindUser;

public class FindUserCommandHandler : IRequestHandler<FindUserCommand, FindUserResponse>
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
    
    public async Task<FindUserResponse> Handle(FindUserCommand command, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId();
        
        var user = await _repository.Entity<Domain.Entities.User>()
            .FirstOrDefaultAsync(u => u.UserName == command.UserName);
        
        var id = user.Id.ToString();

        var chatRoom = await _chatService.GetChatRoom(new List<string> { id });
        if (chatRoom == null)
            chatRoom = new ChatRoom();

        var userName = user.UserName;
        var avatar = user.Avatar;
        var fullName = $"{user.FirstName} {user.LastName}";
        
        var response = new FindUserResponse(
            id,
            chatRoom != null ? chatRoom.Id : null, 
            avatar,
            userName, 
            fullName);
        
        return response;
    }
}