using IoChatServer.Domain.Models;
using IoChatServer.Services.Chat;

namespace IoChatServer.Application.Commands.Chat.GetChatRooms;

public class GetChatRoomsOutput : SucceededResult
{
    public List<ChatRoomDto> ChatRooms { get; set; }
    
    public static GetChatRoomsOutput Success(List<ChatRoomDto> chatRooms) => new(true, chatRooms);
    public static GetChatRoomsOutput Failure(string error) => new(false, null, error);

    public GetChatRoomsOutput(bool succeeded, List<ChatRoomDto> chatRooms, string? error = null)
        : base(succeeded, error)
    {
        ChatRooms = chatRooms;
    }
}