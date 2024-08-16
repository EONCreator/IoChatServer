namespace IoChatServer.Domain.Entities;

public class ChatRoom
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<User> Users { get; set; } = new List<User>();

    public ChatRoom()
    {
        Date = DateTime.UtcNow;
    }
}