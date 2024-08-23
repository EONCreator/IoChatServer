using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Commands.Chat.FindUser;

public class FindUserClientModel
{
    public string Id { get; set; }
    public int? ChatRoomId { get; set; }
    public string? Avatar { get; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public bool Online { get; set; }

    public FindUserClientModel(
        string id,
        int? chatRoomId,
        string? avatar,
        string userName,
        string fullName,
        bool online)
    {
        Id = id;
        ChatRoomId = chatRoomId;
        Avatar = avatar;
        UserName = userName;
        FullName = fullName;
        Online = online;
    }
}

public class FindUserOutput : SucceededResult
{
    public FindUserClientModel User { get; set; }
    
    public static FindUserOutput Success(FindUserClientModel user) => new(true, user);
    public static FindUserOutput Failure(string error) => new(false, null, error);

    public FindUserOutput(bool succeeded, FindUserClientModel user, string? error = null)
        : base(succeeded, error)
    {
        User = user;
    }
}