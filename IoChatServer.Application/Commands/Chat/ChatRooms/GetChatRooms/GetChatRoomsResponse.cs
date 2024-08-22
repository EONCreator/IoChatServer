using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class GetChatRoomsResponse
{
    public List<ChatRoomDto> ChatRooms { get; set; }

    public GetChatRoomsResponse(List<ChatRoomDto> chatRooms)
    {
        ChatRooms = chatRooms;
    }
}