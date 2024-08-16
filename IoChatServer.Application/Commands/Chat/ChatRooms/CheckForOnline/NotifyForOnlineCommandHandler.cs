using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace IoChatServer.Application.Commands.Chat.FindUser;

public class NotifyForOnlineCommandHandler : IRequestHandler<NotifyForOnlineCommand, NotifyForOnlineResponse>
{
    private IRepository _repository;
    private IUserService _userService;
    private IChatService _chatService;
    private IHubContext<ChatHub> _chatHub;
    
    public NotifyForOnlineCommandHandler(
        IRepository repository, 
        IUserService userService,
        IChatService chatService,
        IHubContext<ChatHub> chatHub)
    {
        _repository = repository;
        _userService = userService;
        _chatService = chatService;
        _chatHub = chatHub;
    }
    
    public async Task<NotifyForOnlineResponse> Handle(NotifyForOnlineCommand command, CancellationToken cancellationToken)
    {
        var userId = await _userService.GetCurrentUserId();
        
        var userIds = await _chatService.GetUserIdsOfAllChatRooms();
        
        var connectionIdList = new List<IEnumerable<string>>();
        foreach (var id in userIds)
            connectionIdList.Add(ChatHub.Connections.GetConnections(id));

        var connectionIds = new List<string>();
        foreach (var userConnections in connectionIdList)
        {
            foreach (var connection in userConnections) 
            {
                connectionIds.Add(connection);
            }
        }


        if (ChatHub.Connections.GetConnections(userId).Count() > 0)
        {
            await _chatHub.Clients.Clients(connectionIds).SendAsync("online", userId, true);
            return new NotifyForOnlineResponse(userId, true);
        }
        
        await _chatHub.Clients.Clients(connectionIds).SendAsync("online", userId, false);
        
        return new NotifyForOnlineResponse(userId, false);
    }
}