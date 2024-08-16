using IoChatServer.Domain.Entities;
using IoChatServer.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IoChatServer.Data;

public class AppDbContext 
    : IdentityDbContext<User, Role, Guid>, IRepository
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    public DbSet<ChatRoomUser> UserChatRooms { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ChatRoomUser>().HasKey(cb => new {cb.UserId, cb.ChatRoomId});
    }
    
    public DbSet<TEntity> Entity<TEntity>() where TEntity : class
        => Set<TEntity>();
    
    public Task SaveChanges()
    {
        return SaveChangesAsync();
    }
}