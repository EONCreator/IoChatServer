namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;

public class CreateChatRoomResponse
{
    public int ChatRoomId { get; }
    public string LastMessage { get; }
    public string ChatRoomName { get; }
    public string Avatar { get; }

    public CreateChatRoomResponse(
        int chatRoomId, 
        string lastMessage, 
        string chatRoomName, 
        string avatar)
    {
        ChatRoomId = chatRoomId;
        LastMessage = lastMessage;
        ChatRoomName = chatRoomName;
        Avatar = avatar;
    }
}