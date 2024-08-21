using Microsoft.EntityFrameworkCore;
using IoChatServer.Data;
using IoChatServer.Domain.Repositories;

namespace IoChatServer.Extensions;

public static class Database
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString, 
                    b => b.MigrationsAssembly("IoChatServer")))
            .AddScoped<IRepository>(f => f.GetRequiredService<AppDbContext>());

        return services;
    }
}