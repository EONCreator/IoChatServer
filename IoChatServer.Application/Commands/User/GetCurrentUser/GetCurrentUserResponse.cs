namespace IoChatServer.Application.Commands.User.GetCurrentUser;

public class GetCurrentUserResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    
    public string? Avatar { get; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }

    public GetCurrentUserResponse(Domain.Entities.User user)
    {
        Id = user.Id;
        UserName = user.UserName;

        Avatar = user.Avatar;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }
}