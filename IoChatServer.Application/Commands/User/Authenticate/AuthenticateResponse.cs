namespace IoChatServer.Application.Commands.Authenticate.AuthenticateCommand;

public class AuthenticateResponse
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string Token { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool Succeeded { get; }
    
    public static AuthenticateResponse Failure => new(false, null, null);
    
    public AuthenticateResponse(bool succeeded, Domain.Entities.User user, string token)
    {
        Succeeded = succeeded;
        
        Id = user.Id;
        UserName = user.UserName;
        Token = token;

        FirstName = user.FirstName;
        LastName = user.LastName;
    }
}