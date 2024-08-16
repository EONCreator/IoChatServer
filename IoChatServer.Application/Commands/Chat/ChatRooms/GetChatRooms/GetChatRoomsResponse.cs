using IoChatServer.Domain.Entities;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class ChatRoomUserModel
{
    public int ChatRoomId { get; set; }
    public string Id { get; set; }
    public string? Avatar { get; set; }
    public string ChatRoomName { get; set; }
    public string LastMessage { get; set; }
    public int UnreadMessages { get; set; }
    public bool Online { get; set; }
}

public class GetChatRoomsResponse
{
    public List<ChatRoomUserModel> ChatRooms { get; set; }

    public GetChatRoomsResponse(List<ChatRoomUserModel> chatRooms)
    {
        ChatRooms = chatRooms;
    }
}