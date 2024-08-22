using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.User;

namespace IoChatServer.Services.Chat;
using Domain.Entities;

public interface IChatService
{
    /// <summary>
    /// This method checks if the user is in a chat room
    /// </summary>
    /// <param name="chatRoomId">Chat room ID that is being checked</param>
    /// <returns></returns>
    Task<bool> UserInChatRoom(int chatRoomId);
    
    /// <summary>
    /// This method checks if a chat room exists that contains users with specific IDs.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<bool> ChatRoomIsExists(List<string> ids);
    
    /// <summary>
    /// This method returns a chat room by its ID
    /// </summary>
    /// <param name="chatRoomId">Chat room ID that is being searched</param>
    /// <returns></returns>
    Task<ChatRoom>? GetChatRoom(int chatRoomId);
    
    /// <summary>
    /// This method returns a chat room containing users with specific IDs.
    /// </summary>
    /// <param name="userIds">User IDs for which the check is being performed</param>
    /// <returns></returns>
    Task<ChatRoom>? GetChatRoom(List<string> userIds);
    
    /// <summary>
    /// This method returns all chat rooms that have the current user in them.
    /// </summary>
    /// <returns></returns>
    Task<List<ChatRoomDto>> GetChatRooms();
    
    /// <summary>
    /// This method returns the ID of all users from all chat rooms that the current user is a member of.
    /// </summary>
    /// <returns></returns>
    Task<ICollection<string>> GetUserIdsOfAllChatRooms();

    /// <summary>
    /// This method returns messages from a chat room by its ID.
    /// </summary>
    /// <param name="chatRoomId">ID of the chat room from which you want to return messages</param>
    /// <param name="predicate">Additional expression if needed</param>
    /// <returns></returns>
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

    public async Task<bool> UserInChatRoom(int chatRoomId)
    {
        var userId = await _userService.GetCurrentUserId();

        var userInChatRoom = await _repository.Entity<ChatRoom>()
            .Include(c => c.Users)
            .AnyAsync(c => c.Users.Any(u => u.Id.ToString() == userId));

        return userInChatRoom;
    }
    
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

    public async Task<ChatRoom>? GetChatRoom(int chatRoomId)
        => await _repository.Entity<ChatRoom>()
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == chatRoomId);
    
    public async Task<ChatRoom>? GetChatRoom(List<string> userIds)
    {
        var userId = await _userService.GetCurrentUserId();
        
        var chatRoom = await _repository.Entity<ChatRoom>()
            .FirstOrDefaultAsync(uc =>
                uc.Users.Any(u => u.Id.ToString() == userId)
                && uc.Users.Any(u => userIds.Contains(u.Id.ToString())));

        return chatRoom;
    }
    
    public async Task<List<ChatRoomDto>> GetChatRooms()
    {
        var userId = await _userService.GetCurrentUserId();
        
        var chatRooms = new List<ChatRoomDto>();

        var currentUser = await _repository.Entity<User>()
            .Include(u => u.ChatRooms)
            .ThenInclude(c => c.Users)
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

        foreach (var chatRoom in currentUser.ChatRooms)
        {
            var chatIsGroup = chatRoom.Users.Count > 2; // If chat room's users count > 2, it's group
            foreach (var user in chatRoom.Users.Where(u => u.Id.ToString() != userId))
            {
                var chatRoomName = chatIsGroup ? "Group" : $"{user.FirstName} {user.LastName}";
                
                chatRooms.Add(new ChatRoomDto()
                {
                    Id = user.Id.ToString(),
                    Avatar = user.Avatar,
                    ChatRoomId = chatRoom.Id,
                    ChatRoomName = chatRoomName,
                    
                    LastMessage = _repository.Entity<Message>()
                        .Where(m => m.ChatRoomId == chatRoom.Id).Count() != 0 
                        
                        ? _repository.Entity<Message>()
                            .OrderByDescending(m => m.Date)
                            .FirstOrDefault(m => m.ChatRoomId == chatRoom.Id).Text : "",
                    
                    UnreadMessages = 0,
                    Online = ChatHub.Connections.GetConnections(user.Id.ToString()).Count() != 0
                });

                if (chatIsGroup)
                    break;
            }
        }

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

public class ChatRoomDto
{
    public int ChatRoomId { get; set; }
    public string Id { get; set; }
    public string? Avatar { get; set; }
    public string ChatRoomName { get; set; }
    public string LastMessage { get; set; }
    public int UnreadMessages { get; set; }
    public bool Online { get; set; }
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