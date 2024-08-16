namespace IoChatServer.Domain.Models.Authorization;

using System.ComponentModel.DataAnnotations;

public class AuthenticateRequest
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }

    public AuthenticateRequest(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}