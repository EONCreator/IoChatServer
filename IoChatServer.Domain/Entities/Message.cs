using System.ComponentModel.DataAnnotations.Schema;

namespace IoChatServer.Domain.Entities;

public class Message
{
    public int Id { get; private set;  }
    public string Text { get; private set; }
    public string SenderId { get; private set; }
    public DateTime Date { get; private set; }
    public int ChatRoomId { get; private set; }
    public ChatRoom ChatRoom { get; private set; }
    
    [NotMapped]
    public string SenderName { get; private set; }
    [NotMapped]
    public string SenderAvatar { get; private set; }

    public Message(string text, string senderId, int chatRoomId)
    {
        Text = text;
        SenderId = senderId;
        ChatRoomId = chatRoomId;
        Date = DateTime.UtcNow;
    }
    
    public void SetText(string text) => Text = text;
    public void SetSenderId(string senderId) => SenderId = senderId;
    public void SetDate(DateTime date) => Date = date;
    public void SetChatRoomId(int chatRoomId) => ChatRoomId = chatRoomId;
    public void SetChatRoom(ChatRoom chatRoom) => ChatRoom = chatRoom;
    public void SetSenderName(string senderName) => SenderName = senderName;
    public void SetSenderAvatar(string senderAvatar) => SenderAvatar = senderAvatar;
}