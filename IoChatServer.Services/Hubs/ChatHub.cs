using IoChatServer.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using IoChatServer.Services.Chat;
using IoChatServer.Services.ChatBub;

namespace IoChatServer.Services.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatHub : Hub
{
    public readonly static ConnectionMapping<string> Connections = new();

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

        Clients.Clients(connectionIds).SendAsync(ChatEvents.ONLINE, userId, true).Wait();
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.Claims.ToList().Find(r => r.Type == "id").Value;
        Connections.Remove(userId, Context.ConnectionId);
        
        var userIds = _chatService.GetUserIdsOfAllChatRooms().Result;
        var connectionIds = GetUsersConnections(userIds);

        if (Connections.GetConnections(userId).Count() == 0)
            Clients.Clients(connectionIds).SendAsync(ChatEvents.ONLINE, userId, false).Wait();

        return base.OnDisconnectedAsync(exception);
    }
    
    public static IEnumerable<string> GetUsersConnections(ICollection<string> userIds)
    {
        var connectionIds = new List<string>();
        foreach (var userId in userIds)
        {
            foreach (var connection in Connections.GetConnections(userId))
                connectionIds.Add(connection);
        }

        return connectionIds;
    }
}