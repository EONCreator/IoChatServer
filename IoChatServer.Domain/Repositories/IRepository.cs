using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace IoChatServer.Domain.Repositories;

public interface IRepository
{
    DbSet<TEntity> Entity<TEntity>() 
        where TEntity : class;
    
    Task SaveChanges();
}