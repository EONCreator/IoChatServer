using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using IoChatServer.Data;
using IoChatServer.Domain.Entities;
using IoChatServer.Extensions;
using IoChatServer.Extensions.Controllers;
using IoChatServer.Helpers;
using IoChatServer.Services.Hubs;
using IoChatServer.Services.Authorization;
using IoChatServer.Services.Chat;
using IoChatServer.Services.Images;
using IoChatServer.Services.User;

[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddCorsPolicy();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(ApiBehaviorOptionsConfig.Options)
    .AddJsonOptions(JsonConfig.Options);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IIMageService, ImageService>();

builder.Services.AddDatabase(config.GetConnectionString("DefaultConnection"));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSignalR();
builder.Services.AddJwtAuthentication();

builder.Services.AddMediatRCommandHandlers();

var app = builder.Build();

app.UseCors("MyPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<AppDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    { 
        endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chat");
});

var imagesConfig = config.GetSection("FileStorage")
    .GetSection("Images");
var fileProvider = imagesConfig.GetValue<string>("FileProvider");
var requestPath = imagesConfig.GetValue<string>("RequestPath");

app
    .UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @fileProvider)),
    RequestPath = new PathString(requestPath)
})
    .UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @fileProvider)),
    RequestPath = new PathString(requestPath)
});

app.Run();