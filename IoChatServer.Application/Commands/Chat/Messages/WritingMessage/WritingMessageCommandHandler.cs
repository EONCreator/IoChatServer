using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Application.Commands.Messages.SendMessageCommand;

public class WritingMessageCommandHandler : IRequestHandler<WritingMessageCommand, WritingMessageResponse>
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
    
    public async Task<WritingMessageResponse> Handle(WritingMessageCommand command, CancellationToken cancellationToken)
    {
        List<string> ids = new List<string>();
        
        var userId = await _userService.GetCurrentUserId();

        var chatRoom = await _repository.Entity<ChatRoom>()
            .Include(c => c.Users.Where(u => u.Id.ToString() != userId))
            .FirstOrDefaultAsync(c => c.Id == command.ChatRoomId);

        // Connections of getting users in chat room
        var userToIds = new List<IEnumerable<string>>();
        
        foreach (var user in chatRoom.Users)
            userToIds.Add(ChatHub.Connections.GetConnections(user.Id.ToString()));

        foreach (var user in userToIds)
        {
            foreach (var userConnection in user)
            {
                ids.Add(userConnection);
            }
        }

        await _chatHub.Clients.Clients(ids).SendAsync("writing", command.ChatRoomId);
        
        return new WritingMessageResponse();
    }
}