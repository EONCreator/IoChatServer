using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Commands.Chat.CreateChatRoom;

public class CreateChatRoomClientModel
{
    public int ChatRoomId { get; }
    public string LastMessage { get; }
    public string ChatRoomName { get; }
    public string Avatar { get; }

    public CreateChatRoomClientModel(
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

public class CreateChatRoomOutput : SucceededResult
{
    public CreateChatRoomClientModel ChatRoom { get; set; }
    
    public static CreateChatRoomOutput Success(CreateChatRoomClientModel chatRoom) => new(true, chatRoom);
    public static CreateChatRoomOutput Failure(string error) => new(false, null, error);

    public CreateChatRoomOutput(bool succeeded, CreateChatRoomClientModel chatRoom, string? error = null)
        : base(succeeded, error)
    {
        ChatRoom = chatRoom;
    }
}