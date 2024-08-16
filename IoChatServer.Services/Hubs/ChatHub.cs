using System.Collections;
using System.Security.Claims;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Chat;
using IoChatServer.Services.ChatBub;
using IoChatServer.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IoChatServer.Services.Hubs;
using Microsoft.AspNetCore.SignalR;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub : Hub
{
    public readonly static ConnectionMapping<string> Connections = 
        new ConnectionMapping<string>();

    private IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override Task OnConnectedAsync()
    {
        var userId = Context.User.Claims.ToList().Find(r => r.Type == "id").Value;
        Connections.Add(userId, Context.ConnectionId);
        
        var userIds = _chatService.GetUserIdsOfAllChatRooms().Result;
        var connectionIds = GetUsersConnections(userIds);

        Clients.Clients(connectionIds).SendAsync("online", userId, true).Wait();
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.Claims.ToList().Find(r => r.Type == "id").Value;
        Connections.Remove(userId, Context.ConnectionId);
        
        var userIds = _chatService.GetUserIdsOfAllChatRooms().Result;
        var connectionIds = GetUsersConnections(userIds);

        if (Connections.GetConnections(userId).Count() == 0)
            Clients.Clients(connectionIds).SendAsync("online", userId, false).Wait();

        return base.OnDisconnectedAsync(exception);
    }
    
    public static IEnumerable<string> GetUsersConnections(ICollection<string> userIds)
    {
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

        return connectionIds;
    }
}