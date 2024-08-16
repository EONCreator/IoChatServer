using IoChatServer.Application.Commands.User.CreateUserCommand.User;
using IoChatServer.Services.User;
using MediatR;

namespace IoChatServer.Application.Commands.User.CreateUserCommand;

public class CreateUserCommand : IRequest<SignInResult>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
}