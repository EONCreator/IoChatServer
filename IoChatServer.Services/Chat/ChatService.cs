using System.Collections;
using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using IoChatServer.Services.User;
using Microsoft.EntityFrameworkCore;

namespace IoChatServer.Services.Chat;

public interface IChatService
{
    Task<ChatRoom>? GetChatRoom(List<string> ids);
    Task<ICollection<ChatRoom>> GetChatRooms();
    Task<ICollection<string>> GetUserIdsOfAllChatRooms();
    
    Task<bool> ChatRoomIsExists(List<string> ids);
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

    public async Task<bool> ChatRoomIsExists(List<string> ids)
    {
        var userId = await _userService.GetCurrentUserId();

        return await _repository.Entity<ChatRoom>()
            .AnyAsync(uc =>
                uc.Users.Any(u => u.Id.ToString() == userId)
                && uc.Users.Any(u => ids.Where(c => c != userId.ToString())
                    .Contains(u.Id.ToString())));
    }
}