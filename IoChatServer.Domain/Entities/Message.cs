using System.ComponentModel.DataAnnotations.Schema;

namespace IoChatServer.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string SenderId { get; set; }
    public DateTime Date { get; set; }
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; }
    
    [NotMapped]
    public string SenderName { get; set; }
    [NotMapped]
    public string SenderAvatar { get; set; }

    public Message(string text, string senderId, int chatRoomId)
    {
        Text = text;
        SenderId = senderId;
        ChatRoomId = chatRoomId;
        Date = DateTime.UtcNow;
    }
}