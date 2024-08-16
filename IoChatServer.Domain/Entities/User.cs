using Microsoft.AspNetCore.Identity;

namespace IoChatServer.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string? Avatar { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Address { get; private set; }
    
    public int UnreadMessages { get; private set; }
    public bool IsOnline { get; private set; }
    
    public List<ChatRoom> ChatRooms { get; set; }

    public void SetAvatar(string avatar) => Avatar = avatar;
    public void SetFirstName(string firstName) => FirstName = firstName;
    public void SetLastName(string lastName) => LastName = lastName;
    public void SetAddress(string address) => Address = address;
}