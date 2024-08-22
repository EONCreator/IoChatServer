using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.User;

namespace IoChatServer.Services.Chat;

public interface IChatService
{
    Task<ChatRoom>? GetChatRoom(List<string> ids);
    Task<ICollection<ChatRoom>> GetChatRooms();
    Task<ICollection<string>> GetUserIdsOfAllChatRooms();
    
    Task<bool> ChatRoomIsExists(List<string> ids);

    Task<List<MessageDto>> GetMessagesOfChatRoom(int chatRoomId, 
        Expression<Func<MessageDto, bool>>? predicate = null);
}

public class ChatService : IChatService
{
    private readonly IUserService _userService;
    private readonly IRepository _repository;

    public ChatService(IUserService userService, IRepository repository)
    {
        _userService = userService;
        _repository = repository;
    }
    #region ChatRooms
    
    public async Task<bool> ChatRoomIsExists(
        List<string> ids)
    {
        var userId = await _userService.GetCurrentUserId();

        return await _repository.Entity<ChatRoom>()
            .AnyAsync(uc =>
                uc.Users.Any(u => u.Id.ToString() == userId)
                && uc.Users.Any(u => ids.Where(c => c != userId.ToString())
                    .Contains(u.Id.ToString())));
    }
    
    public async Task<ChatRoom>? GetChatRoom(List<string> ids)
    {
        var userId = await _userService.GetCurrentUserId();
        
        var chatRoom = await _repository.Entity<ChatRoom>()
            .FirstOrDefaultAsync(uc =>
                uc.Users.Any(u => u.Id.ToString() == userId)
                && uc.Users.Any(u => ids.Contains(u.Id.ToString())));

        return chatRoom;
    }
    
    public async Task<ICollection<ChatRoom>> GetChatRooms()
    {
        var userId = await _userService.GetCurrentUserId();
        
        var user = await _repository.Entity<Domain.Entities.User>()
            .Include(c => c.ChatRooms)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        var chatRooms = new List<ChatRoom>();
        foreach (var chatRoom in user.ChatRooms)
            chatRooms.Add(chatRoom);

        return chatRooms;
    }

    public async Task<ICollection<string>> GetUserIdsOfAllChatRooms()
    {
        var userId = await _userService.GetCurrentUserId();

        var userIds = new List<string>();

        var chatRooms = await _repository.Entity<ChatRoom>()
            .Select(c => new
            {
                c.Id,
                Users = c.Users.Select(u => new
                    {
                        u.Id
                    })
                    .Distinct()
            })
            .Where(u => u.Users.Any(u => u.Id.ToString() == userId))
            .ToListAsync();

        foreach (var chatRoom in chatRooms)
        {
            foreach (var user in chatRoom.Users.Where(u => u.Id.ToString() != userId))
            {
                userIds.Add(user.Id.ToString());
            }
        }

        return await Task.FromResult(userIds);
    }

    #endregion
    
    #region Messages
    
    public async Task<List<MessageDto>> GetMessagesOfChatRoom(
        int chatRoomId,
        Expression<Func<MessageDto, bool>>? predicate = null
        )
    {
        var messagesQuery = _repository.Entity<Message>()
            .Select(m => new MessageDto
            {
                Id = m.Id,
                ChatRoomId = m.ChatRoomId,
                Text = m.Text,
                Date = m.Date,
                SenderId = m.SenderId,
                Sender = _repository.Entity<Domain.Entities.User>()
                    .Select(u => new SenderDto
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Avatar = u.Avatar
                    })
                    .FirstOrDefault(u => u.Id.ToString() == m.SenderId)
            })
            .Where(m => m.ChatRoomId == chatRoomId);
        
        if (predicate != null)
            messagesQuery = messagesQuery.AsQueryable().Where(predicate); // Применяем предикат
        
        var messages = await messagesQuery.ToListAsync();
        return messages;
    }
    
    #endregion
}

public class MessageModel
{
    public int Id { get; }
    public string Text { get;  }
    public DateTime Date { get; }
    public string SenderId { get; }
    public string SenderName { get; }
    public string SenderAvatar { get; }

    public MessageModel(
        int id, 
        string text, 
        DateTime date, 
        string senderId,
        string senderName,
        string senderAvatar)
    {
        Id = id;
        Text = text;
        Date = date;
        SenderId = senderId;
        SenderName = senderName;
        SenderAvatar = senderAvatar;
    }
}

public class MessageDto
{
    public int Id { get; set; }
    public int ChatRoomId { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public string SenderId { get; set; }
    public SenderDto Sender { get; set; }
}

public class SenderDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
}