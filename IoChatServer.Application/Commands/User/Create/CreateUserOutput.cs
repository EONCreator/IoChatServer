namespace IoChatServer.Application.Commands.User.CreateUserCommand.User;

public class CreateUserOutput
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Token { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public CreateUserOutput(Domain.Entities.User user, string token)
    {
        Id = user.Id;
        UserName = user.UserName;
        Token = token;

        FirstName = user.FirstName;
        LastName = user.LastName;
    }
}