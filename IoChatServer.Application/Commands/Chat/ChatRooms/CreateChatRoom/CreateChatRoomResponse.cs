using IoChatServer.Domain.Entities;

namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;

public class CreateChatRoomResponse
{
    public int ChatRoomId { get; set; }
    public string LastMessage { get; set; }
    public string ChatRoomName { get; set; }
    public string Avatar { get; }

    public CreateChatRoomResponse(int chatRoomId, string lastMessage, string chatRoomName, string avatar)
    {
        ChatRoomId = chatRoomId;
        LastMessage = lastMessage;
        ChatRoomName = chatRoomName;
        Avatar = avatar;
    }
}