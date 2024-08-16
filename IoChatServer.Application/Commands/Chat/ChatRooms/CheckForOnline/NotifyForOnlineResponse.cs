namespace IoChatServer.Application.Commands.Chat.FindUser;

public class NotifyForOnlineResponse
{
    public string UserId { get; set; }
    public bool Online { get; set; }

    public NotifyForOnlineResponse(string userId, bool online)
    {
        UserId = userId;
        Online = online;
    }
}