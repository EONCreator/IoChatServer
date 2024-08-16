namespace IoChatServer.Application.Commands.Chat.FindUser;

public class FindUserResponse
{
    public string Id { get; set; }
    public int? ChatRoomId { get; set; }
    public string? Avatar { get; }
    public string UserName { get; set; }
    public string FullName { get; set; }

    public FindUserResponse(string id, int? chatRoomId, string? avatar, string? userName, string fullName)
    {
        Id = id;
        ChatRoomId = chatRoomId;
        Avatar = avatar;
        UserName = userName;
        FullName = fullName;
    }
}