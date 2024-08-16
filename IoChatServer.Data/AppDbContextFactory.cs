using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IoChatServer.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("/home/eoncreator/RiderProjects/Jupiter/Jupiter/appsettings.json")
            .Build();

        // Criando o DbContextOptionsBuilder manualmente
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        // cria a connection string. 
        // requer a connectionstring no appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseNpgsql(connectionString);

        // Cria o contexto
        return new AppDbContext(builder.Options);
    }
}