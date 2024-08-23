namespace IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;

public class AuthenticateOutput
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Token { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool Succeeded { get; }
    
    public static AuthenticateOutput Failure => new(false, null, null);
    
    public AuthenticateOutput(bool succeeded, Domain.Entities.User user, string token)
    {
        Succeeded = succeeded;
        
        Id = user.Id;
        UserName = user.UserName;
        Token = token;

        FirstName = user.FirstName;
        LastName = user.LastName;
    }
}