namespace IoChatServer.Domain.Entities;

public class ChatRoomUser
{
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; }
}