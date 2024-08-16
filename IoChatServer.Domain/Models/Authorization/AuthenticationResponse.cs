using IoChatServer.Domain.Entities;

namespace IoChatServer.Domain.Models.Authorization;

public class AuthenticateResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        UserName = user.UserName;
        Token = token;
    }
}