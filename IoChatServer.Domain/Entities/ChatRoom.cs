namespace IoChatServer.Domain.Entities;

public class ChatRoom
{
    public int Id { get; private set; }
    public DateTime Date { get; private set; }
    public string? Name { get; private set; }
    public string? Avatar { get; private set; }
    public List<User> Users { get; private set; } = new();
    public string? LastMessage { get; private set; }
    public DateTime LastMessageDate { get; private set; }
    public int UnreadMessages { get; private set; }

    public ChatRoom()
    {
        var utcNow = DateTime.UtcNow;
        Date = utcNow;
        LastMessageDate = utcNow;
    }
    
    public void SetName(string name) => Name = name;
    public void SetAvatar(string avatar) => Avatar = avatar;
    public void SetLastMessage(string lastMessage) => LastMessage = lastMessage;
    public void SetLastMessageDateTime(DateTime lastMessageDate) => LastMessageDate = lastMessageDate;
    public void SetUnreadMessages(int count) => UnreadMessages = count;
}