using IoChatServer.Domain.Models;

namespace IoChatServer.Application.Queries.User.GetCurrentUser;

using Domain.Entities;

public class GetCurrentUserOutput : SucceededResult
{
    public User User { get; set; }
    
    public static GetCurrentUserOutput Succeeded(User user) => new(true, user);
    public static GetCurrentUserOutput Failure(string error) => new(false, null, error);

    public GetCurrentUserOutput(bool succeeded, User user, string? error = null)
        : base(succeeded, error)
    {
        User = user;
    }
}