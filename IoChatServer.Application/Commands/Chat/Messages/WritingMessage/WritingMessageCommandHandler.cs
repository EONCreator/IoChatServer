using IoChatServer.Abstractions;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Helpers.Errors;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class WritingMessageCommandHandler : IRequestHandler<WritingMessageCommand, WritingMessageOutput>
{
    private IUserService _userService;
    private IHubContext<ChatHub> _chatHub;
    private IRepository _repository;
    
    public WritingMessageCommandHandler(IUserService userService, IHubContext<ChatHub> chatHub,
        IRepository repository)
    {
        _userService = userService;
        _chatHub = chatHub;
        _repository = repository;
    }
    
    public async Task<WritingMessageOutput> Handle(WritingMessageCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();

        var chatRoom = await _repository.Entity<ChatRoom>()
            .Include(c => c.Users.Where(u => u.Id.ToString() != userId))
            .FirstOrDefaultAsync(c => c.Id == command.ChatRoomId);
        
        if (chatRoom == null)
            return WritingMessageOutput.Failure(ChatErrors.DoesNotExists);

        List<string> userToIds = new List<string>();
        foreach (var user in chatRoom.Users)
            userToIds.Add(user.Id.ToString());

        await _chatHub.Clients.Clients(ChatHub.GetUsersConnections(userToIds))
            .SendAsync(ChatEvents.WRITING, command.ChatRoomId);
        
        return WritingMessageOutput.Success();
    }
}